using Cybtans.Serialization;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Messaging.RabbitMQ
{
    public sealed class RabbitMessageQueue : IMessageQueue, IDisposable
    {       
        private bool _disposed;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMessageQueue>? _logger;
        private IConnection? _connection;
        private IModel? _publishChannel;
        private IModel? _consumerChannel;
        private IBasicProperties? _properties;
        private object sync_root = new object();
        private RabbitMessageQueueOptions _options;
        private QueueDeclareOk? _queue;
        private string? _queueName;
        private bool _queueLocked;
        private EventingBasicConsumer? _consumer;
        private bool _started;
        private readonly HashSet<string> _publishExchanges = new HashSet<string>();
        private readonly HashSet<string> _consumeExchanges = new HashSet<string>();
        private readonly MessageSubscriptionManager _subscriptionManager;
        private readonly IMessageSerializer _messageSerializer;

        public event EventHandler? Started;

        public RabbitMessageQueue(IConnectionFactory connectionFactory, MessageSubscriptionManager subscriptionManager, RabbitMessageQueueOptions? options = null, ILogger<RabbitMessageQueue>? logger = null, IMessageSerializer messageSerializer = null)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _options = options ?? new RabbitMessageQueueOptions();
            _subscriptionManager = subscriptionManager;
            _messageSerializer = messageSerializer ?? new CybtansMessageSerializer();
        }

        public RabbitMessageQueue(IConnectionFactory connectionFactory, RabbitMessageQueueOptions? options = null, ILogger<RabbitMessageQueue>? logger = null, IMessageSerializer messageSerializer= null)            
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _options = options ?? new RabbitMessageQueueOptions();
            _subscriptionManager = new MessageSubscriptionManager(null, _options.Exchange.Name);
            _messageSerializer = messageSerializer ?? new CybtansMessageSerializer();
        }

        public bool IsConnected =>  _connection != null && _connection.IsOpen && !_disposed;
       
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;          

            try
            {
                _publishChannel?.Close();
                _consumerChannel?.Close();
                _connection?.Close();                
            }
            catch(IOException ex)
            {
                _logger?.LogError(ex, ex.Message);                
            }            
        }

        private void OpenConnection()
        {
            if (IsConnected)
                return;

            _logger?.LogInformation($"RabbitMQ Client is connecting to {_connectionFactory.Uri}");
            
            lock (sync_root)
            {
                if (IsConnected)
                    return;

                var policy = Policy.Handle<SocketException>()
                        .Or<BrokerUnreachableException>()
                        .WaitAndRetry(_options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            _logger?.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                        }
                    );

                policy.Execute(() =>
                {                    
                    _connection = _connectionFactory.CreateConnection();
                });

                if (_connection == null)
                {
                    _logger?.LogError("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    throw new QueueConnectionException("RabbitMQ connection is not created");
                }              

                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger?.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);
                                 
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
                if (_options.Exchange.PersistMessages)
                {
                    _properties.DeliveryMode = 2; // persistent                        
                }

                if (_options.Exchange.BradcastAlternateExchange != null)
                {
                    _publishChannel.ExchangeDeclare(_options.Exchange.BradcastAlternateExchange, "fanout");
                }

                _publishChannel.CallbackException += (sender, ea) =>
                  {
                      _logger?.LogWarning(ea.Exception, "Recreating RabbitMQ publishing channel");

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

                if (_options.Queue.PrefetchCount != null)
                {
                    _consumerChannel.BasicQos(0, _options.Queue.PrefetchCount.Value, false);
                }

                _consumerChannel.CallbackException += (sender, ea) =>
                {
                    _logger?.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                    _consumerChannel?.Dispose();
                    _consumerChannel = null;

                    CreateConsumerChannel();
                };

                try
                {
                    Dictionary<string, object>? args = null;
                    if(_options.Queue.DeadLetterExchange != null)
                    {
                        args ??= new Dictionary<string, object>();

                        args["x-dead-letter-exchange"] = _options.Queue.DeadLetterExchange;
                    }

                    _queue = _consumerChannel.QueueDeclare(_options.Queue.Name ?? "",
                        _options.Queue.Durable,
                        _options.Queue.Exclusive, 
                        _options.Queue.AutoDelete, 
                        args);

                    _queueName = _queue.QueueName;
                    _queueLocked = false;
                }
                catch (OperationInterruptedException ex) when (ex.ShutdownReason.ReplyCode == 405)
                {
                    //resource-locked
                    _queueName = null;
                    _queueLocked = true;
                    _logger?.LogWarning(ex, $"RabbitMQ Queue is locked {_options.Queue}, Error:{ex.ShutdownReason.ReplyText}");
                }
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
                if (_options.Exchange.BradcastAlternateExchange != null)
                {
                    args = new Dictionary<string, object>
                    {
                        ["alternate-exchange"] = _options.Exchange.BradcastAlternateExchange
                    };
                }

                _publishChannel!.ExchangeDeclare(exchange, type: _options.ExchangeType, durable: _options.Exchange.Durable, autoDelete: _options.Exchange.AutoDelete ,arguments: args);
                _publishExchanges.Add(exchange);
            }
        }

        private void AddSubscriptionExchange(string exchange)
        {          
            if(_consumerChannel == null)
            {
                CreateConsumerChannel();
            }

            if (_queueLocked)
                return;

            if (!_consumeExchanges.Contains(exchange))
            {
                _consumerChannel!.ExchangeDeclare(exchange, 
                    type: _options.ExchangeType, 
                    durable: _options.Exchange.Durable, 
                    autoDelete: _options.Exchange.AutoDelete);

                _consumeExchanges.Add(exchange);
            }
        }

        public Task Publish(byte[] bytes, string exchange, string topic)
        {            
            return Task.Run(() => PublishInternal(exchange, topic, bytes));
        }

        public Task Publish(object message, string? exchange, string? topic)
        {
            if (exchange == null || topic == null)
            {
                Type type = message.GetType();
                var binding = _subscriptionManager.GetBindingForType(type, exchange, topic);
                if (binding == null)
                {
                    throw new QueuePublishException($"Exchange not found for {message.GetType()}", message);
                }
                exchange = binding.Exchange;
                topic = binding.Topic;
            }

            var bytes = _messageSerializer.Serialize(message);
            return Task.Run(() => PublishInternal(exchange,  topic, bytes));
        }       

        private void PublishInternal(string exchange, string topic, byte[] data)
        {
            AddPublishExchange(exchange);

            if (_publishChannel == null)
                throw new QueuePublishException("RabbitMQ publish channel not created");

            _logger?.LogDebug("Publishing Message {Exchange} {Topic}", exchange, topic);

            _publishChannel.BasicPublish(exchange, topic, false, _properties, data);

            _logger?.LogDebug("Message Published {Exchange} {Topic}", exchange, topic);
        }

        public void Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
          where THandler : IMessageHandler<TMessage>
        {
           var binding = _subscriptionManager.Subscribe<TMessage, THandler>(exchange, topic);
            if (_started)
            {
                SubscribeInternal(binding);
            }
        }

        public void Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? exchange = null, string? topic = null)
        {
            var binding = _subscriptionManager.Subscribe<TMessage>(handler, exchange, topic);
            if (_started)
            {
                SubscribeInternal(binding);
            }
        }

        private void SubscribeInternal(BindingInfo subsInfo)
        {
            AddSubscriptionExchange(subsInfo.Exchange);
            if (_consumerChannel == null)
                throw new QueueSubscribeException("RabbitMQ consumer channel not created");

            if (_queueLocked)            
                return;            
            
            _consumerChannel.QueueBind(_queueName, subsInfo.Exchange, subsInfo.Topic);

            lock (sync_root)
            {
                if (_consumer == null)
                {
                    _consumer = new EventingBasicConsumer(_consumerChannel);
                    _consumer.Received += Consumer_Received;

                    _consumerChannel.BasicConsume(
                        queue: _queueName,
                        autoAck: _options.Queue.AutoAck,
                        consumer: _consumer);
                }
            }
        }

        public void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null)
             where THandler : IMessageHandler<TMessage>
        {
            _subscriptionManager.Unsubscribe<TMessage, THandler>(exchange, topic);
        }

        public BindingInfo? GetBinding(Type type, string? topic)
        {
            return _subscriptionManager.GetBindingForType(type, null, topic);
        }

        public void RegisterBinding<T>(string exchage, string? topic = null)
        {
            _subscriptionManager.RegisterBinding<T>(exchage, topic);
        }

        public void Start()
        {
            foreach (var binding in _subscriptionManager.GetBindings())
            {
                SubscribeInternal(binding);
            }

            Started?.Invoke(this, EventArgs.Empty);
            _started = true;
        }

        #region Event Handlers

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger?.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            OpenConnection();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger?.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            OpenConnection();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger?.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            OpenConnection();
        }

        private void Consumer_Received(object? sender, BasicDeliverEventArgs args)
        {
            var exchage = args.Exchange;
            var topic = args.RoutingKey;
            var data = args.Body;
            var deliveryTag = args.DeliveryTag;

            _logger?.LogDebug("Message Received {Exchange} {Topic}", exchage, topic);

            Task.Run(async () =>
            {
                try
                {
                    _logger?.LogDebug("Message Dispatched {Exchange} {Topic}", exchage, topic);

                    await _subscriptionManager.HandleMessage(exchage, topic, data, _messageSerializer).ConfigureAwait(false);
                    if (deliveryTag > 0)
                    {                        
                        _consumerChannel?.BasicAck(deliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, ex.Message);
                    _consumerChannel?.BasicNack(deliveryTag, false, _options.Queue.NackRequeue);
                }
            });
        }

     

        #endregion
    }
}
