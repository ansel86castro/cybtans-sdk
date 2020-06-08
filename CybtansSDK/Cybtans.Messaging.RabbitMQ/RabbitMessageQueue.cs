using Cybtans.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Messaging.RabbitMQ
{

    public sealed class RabbitMessageQueue : IMessageQueue, IDisposable
    {     
        private bool _disposed;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMessageQueue> _logger;
        private IConnection? _connection;
        private IModel? _publishChannel;
        private IModel? _consumerChannel;
        private IBasicProperties? _properties;
        private object sync_root = new object();
        private RabbitMessageQueueOptions _options;
        private string? _queueName;
        private readonly HashSet<string> _publishExchanges = new HashSet<string>();
        private readonly HashSet<string> _consumeExchanges = new HashSet<string>();
        private readonly SubscriptionManager _subscriptionManager;

        public RabbitMessageQueue(IConnectionFactory connectionFactory, 
            SubscriptionManager subscriptionManager, ILogger<RabbitMessageQueue> logger, IOptions<RabbitMessageQueueOptions>options)
        {
            _connectionFactory = connectionFactory;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
            _options = options.Value;
        }

        public bool IsConnected =>  _connection != null && _connection.IsOpen && !_disposed;
       
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            try
            {
                _publishChannel?.Dispose();
                _connection?.Dispose();
            }
            catch(IOException ex)
            {
                _logger.LogCritical(ex, ex.Message);                
            }            
        }

        private void OpenConnection()
        {
            if (IsConnected)
                return;

            _logger.LogInformation($"RabbitMQ Client is connecting to {_connectionFactory.Uri}");
            
            lock (sync_root)
            {
                if (IsConnected)
                    return;

                var policy = RetryPolicy.Handle<SocketException>()
                        .Or<BrokerUnreachableException>()
                        .WaitAndRetry(_options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                        }
                    );

                policy.Execute(() =>
                {                    
                    _connection = _connectionFactory.CreateConnection();
                });

                if (_connection == null)
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    throw new QueueConnectionException("RabbitMQ connection is not created");
                }              

                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);
                                 
            }
        }

        private void CreatePublishChannel()
        {          
            OpenConnection();            

            lock (sync_root)
            {
                if (_publishChannel != null && _publishChannel.IsOpen)
                    return;                

                _publishChannel = _connection!.CreateModel();
                if (_publishChannel == null)
                    throw new QueuePublishException("RabbitMQ publish channel is not created");

                _properties = _publishChannel.CreateBasicProperties();
                if (_options.Durable)
                {
                    _properties.DeliveryMode = 2; // persistent                        
                }

                if (_options.BradcastAlternateExchange != null)
                {
                    _publishChannel.ExchangeDeclare(_options.BradcastAlternateExchange, "fanout");
                }

                _publishChannel.CallbackException += (sender, ea) =>
                  {
                      _logger.LogWarning(ea.Exception, "Recreating RabbitMQ publishing channel");

                      _publishChannel.Dispose();
                      _publishChannel = null;

                      CreatePublishChannel();
                  };
            }
        }

        private void CreateConsumerChannel()
        {
           
            OpenConnection();
            
            lock (sync_root)
            {
                if (_consumerChannel != null && _consumerChannel.IsOpen)
                    return;

                EnsureConnectionCreated();

                _consumerChannel = _connection!.CreateModel();
                if (_consumerChannel == null)
                    throw new QueueSubscribeException("RabbitMQ consume channel is not created");

                _consumerChannel.CallbackException += (sender, ea) =>
                {
                    _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                    _consumerChannel.Dispose();
                    _consumerChannel = null;

                    CreateConsumerChannel();
                };

                var queue = _consumerChannel.QueueDeclare(_options.QueueName ?? "", true, _options.Exclusive, true, null);
                _queueName = queue.QueueName;

                 var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);

            }
        }            

        private void EnsureConnectionCreated()
        {
            if (_connection == null)
            {
                throw new QueueConnectionException("RabbitMQ connection is not created");
            }
        }

        private void AddPublishExchange(string exchange)
        {          
            if(_publishChannel == null)
            {
                CreatePublishChannel();
            }

            if (!_publishExchanges.Contains(exchange))
            {
                Dictionary<string, object>? args = null;
                if (_options.BradcastAlternateExchange != null)
                {
                    args = new Dictionary<string, object>
                    {
                        ["alternate-exchange"] = _options.BradcastAlternateExchange
                    };
                }
                _publishChannel.ExchangeDeclare(exchange, type: _options.ExchangeType, durable: _options.Durable, arguments: args);
                _publishExchanges.Add(exchange);
            }
        }

        private void AddSubscriptionExchange(string exchange)
        {
          
            if(_consumerChannel == null)
            {
                CreateConsumerChannel();
            }

            if (!_consumeExchanges.Contains(exchange))
            {
                _consumerChannel.ExchangeDeclare(exchange, type: _options.ExchangeType, durable: _options.Durable);
                _consumeExchanges.Add(exchange);
            }
        }
     
        public Task Publish(object message, string? exchange, string? topic)
        {
            if (exchange == null || topic == null)
            {
                if (!_subscriptionManager.GetExchangeValues(message.GetType(), out exchange, out topic))
                {
                    if (message is IMessage msg)
                    {
                        exchange = msg.Exchange;
                        topic = msg.Topic;
                    }
                    else
                    {
                        throw new QueuePublishException($"Exchange not found for {message.GetType()}", message);
                    }
                }
            }            

            var bytes = BinaryConvert.Serialize(message);
            return Task.Run(() => PublishInternal(exchange!, topic!, bytes));
        }

        //public Task PublishAll(IEnumerable<object> messages)
        //{
        //    return Task.Run(() =>
        //    {
        //        foreach (var message in messages)
        //        {                    
        //            if (!_subscriptionManager.GetExchangeValues(message.GetType(), out var exchange, out var topic))
        //            {
        //                if (message is IMessage msg)
        //                {
        //                    exchange = msg.Exchange;
        //                    topic = msg.Topic;
        //                }
        //                else
        //                {
        //                    throw new QueuePublishException($"Exchange not found for {message.GetType()}", message);
        //                }
        //            }

        //            var bytes = BinaryConvert.Serialize(message);
        //            PublishInternal(exchange!, topic!, bytes);
        //        }               
        //    });
        //}

        private void PublishInternal(string exchange, string topic, byte[] data)
        {
            AddPublishExchange(exchange);

            if (_publishChannel == null)
                throw new QueuePublishException("RabbitMQ publish channel not created");

            _publishChannel.BasicPublish(exchange, topic, false, _properties, data);
        }

        public void Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
          where THandler : IMessageHandler<TMessage>
        {
            var subsInfo = _subscriptionManager.Subscribe<TMessage, THandler>(exchange, topic);
            SubscribeInternal(subsInfo);
        }

        private void SubscribeInternal<TMessage>(SubscriptionInfo<TMessage> subsInfo)
        {
            AddSubscriptionExchange(subsInfo.Exchange);
            if (_consumerChannel == null)
                throw new QueueSubscribeException("RabbitMQ consumer channel not created");

            _consumerChannel.QueueBind(_queueName, subsInfo.Exchange, subsInfo.Topic);         
        }

        public void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
             where THandler : IMessageHandler<TMessage>
        {
            _subscriptionManager.Unsubscribe<TMessage, THandler>(exchange, topic);
        }

        #region Event Handlers

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            OpenConnection();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            OpenConnection();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            OpenConnection();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs args)
        {
            try
            {
                await _subscriptionManager.HandleMessage(args.Exchange, args.RoutingKey, args.Body.ToArray());
                _consumerChannel?.BasicAck(args.DeliveryTag, multiple: false);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);

                _consumerChannel?.BasicNack(args.DeliveryTag, false, true);

            }

        }

        #endregion
    }
}
