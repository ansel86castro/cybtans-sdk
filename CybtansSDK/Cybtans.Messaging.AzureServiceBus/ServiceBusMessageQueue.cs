using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Messaging.AzureServiceBus
{

    public class ServiceBusMessageQueue : IMessageQueue, IAsyncDisposable
    {
        private readonly ServiceBusClient _sbClient;
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly ILogger<ServiceBusMessageQueue>? _logger;
        private readonly IMessageSerializer _serializer ;
        private readonly ServiceBusOptions _options ;
        private readonly MessageSubscriptionManager _subscriptionManager;

        private bool _disposed;
        private bool _started;
        private object sync_root = new object();
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private Dictionary<string, ServiceBusSender> _senders = new ();
        private Dictionary<string, ServiceBusProcessor> _receivers = new ();

        public event EventHandler? Started;

        public ServiceBusMessageQueue(ServiceBusOptions options, ILogger<ServiceBusMessageQueue>? logger, IMessageSerializer serializer)
            :this(options, new MessageSubscriptionManager(null, options.Topic.Name), logger, serializer)
        {
            
        }

        public ServiceBusMessageQueue(ServiceBusOptions options, MessageSubscriptionManager subscriptionManager, ILogger<ServiceBusMessageQueue>? logger, IMessageSerializer serializer)
        {
            _sbClient = new ServiceBusClient(options.ConnectionString);
            _adminClient = new ServiceBusAdministrationClient(options.ConnectionString);
            _options = options;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
            _serializer  = serializer ?? new CybtansMessageSerializer();
        }

        private ServiceBusSender CreateSender(string? exchange)
        {
            if (exchange == null)
                exchange = _options.Topic.Name;
            
            lock (sync_root)
            {
                if (!_senders.TryGetValue(exchange, out var sender))
                {
                    if (_options.Topic.Create && !_adminClient.TopicExistsAsync(exchange).Result)
                    {
                        _logger?.LogDebug("Topic not exist, creating Topic {Topic}", exchange);

                        try
                        {
                            _adminClient.CreateTopicAsync(exchange).Wait();

                            _logger?.LogDebug("Topic {Topic} created successfully", exchange);
                        }
                        catch(ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
                        {
                            _logger?.LogDebug("Topic {Topic} already exist", exchange);
                        }                       
                    }                    

                    sender = _sbClient.CreateSender(exchange);
                    _senders.Add(exchange, sender);
                }
                return sender;
            }
        }

    
        public void Dispose()
        {
            if (_disposed)
                return;

            foreach (var kv in _receivers)
            {
                kv.Value.StartProcessingAsync().Wait();
            }

            _sbClient.DisposeAsync().GetAwaiter().GetResult();
            _disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            foreach (var kv in _receivers)
            {
                await kv.Value.StartProcessingAsync();
            }

            await _sbClient.DisposeAsync();
            _disposed = true;
        }

        public void Start()
        {
            StartAsync().Wait();
        }

        public async Task StartAsync()
        {
            foreach (var binding in _subscriptionManager.GetBindings())
            {
                await SubscribeInternal(binding);
            }

            Started?.Invoke(this, EventArgs.Empty);
            _started = true;
        }


        public BindingInfo GetBinding(Type type, string topic)
        {
            return _subscriptionManager.GetBindingForType(type, null, topic);
        }

        public void RegisterBinding<T>(string exchage, string? topic = null)
        {
            _subscriptionManager.RegisterBinding<T>(exchage, topic);
        }

        public async Task Publish(byte[] bytes, string? exchange, string topic)
        {
            var sender = CreateSender(exchange);
            var message = new ServiceBusMessage()
            {
                Body = new BinaryData(bytes),
                CorrelationId = topic                 
            };
            message.ApplicationProperties.Add("exchange", exchange ?? _options.Topic.Name);
            message.ApplicationProperties.Add("topic", topic);
            message.ContentType = _serializer.ContentType;

            _logger?.LogDebug("Publishing Message {Exchange} {Topic}", exchange, topic);

            await sender.SendMessageAsync(message).ConfigureAwait(false);

            _logger?.LogDebug("Message Published {Exchange} {Topic}", exchange, topic);
        }

        public Task Publish(object message, string? exchange, string? topic)
        {
            if (exchange == null || topic == null)
            {
                exchange ??= _options.Topic.Name;
                Type type = message.GetType();
                var binding = _subscriptionManager.GetBindingForType(type, exchange, topic);
                if (binding == null)
                {
                    throw new QueuePublishException($"Binding info not found for {message.GetType()}", message);
                }
                exchange = binding.Exchange;
                topic = binding.Topic;
            }

            var bytes = _serializer.Serialize(message);
            return Publish(bytes, exchange, topic);
        }
     
        
        public void Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null) where THandler : IMessageHandler<TMessage>
        {
            if (exchange == null && _options.Subscription?.Name == null)
                throw new InvalidOperationException("Subscription name not defined");

            var binding = _subscriptionManager.Subscribe<TMessage, THandler>(exchange ?? _options.Subscription.Name, topic);
            if (_started)
            {
                SubscribeInternal(binding).Wait();
            }
        }

        public void Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? exchange = null, string? topic = null)
        {
            if (exchange == null && _options.Subscription?.Name == null)
                throw new InvalidOperationException("Subscription name not defined");

            var binding = _subscriptionManager.Subscribe(handler, exchange ?? _options.Subscription.Name, topic);
            if (_started)
            {
                SubscribeInternal(binding).Wait();
            }          
        }

        public void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null) where THandler : IMessageHandler<TMessage>
        {
            if (exchange == null && _options.Subscription?.Name == null)
                throw new InvalidOperationException("Subscription name not defined");

            _subscriptionManager.Unsubscribe<TMessage, THandler>(exchange ?? _options.Subscription.Name, topic);
        }


        private async Task SubscribeInternal(BindingInfo binding)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                if (_options.Subscription.Name == null)
                {
                    _options.Subscription.Name = Guid.NewGuid().ToString();
                }

                var subscriptionName = _options.Subscription.Name;

                if (!_receivers.TryGetValue(binding.Exchange, out var processor))
                {
                    if (_options.Subscription.Create && ! await _adminClient.SubscriptionExistsAsync(binding.Exchange, subscriptionName))
                    {
                        await CreateTopicForSubscriptionAsync(binding.Exchange);
                        try
                        {
                            await _adminClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(binding.Exchange, subscriptionName)
                            {
                                MaxDeliveryCount = _options.Subscription.MaxDeliveryCount,
                            });
                        }
                        catch (ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
                        { }
                    }

                    processor = _sbClient.CreateProcessor(binding.Exchange, subscriptionName, new ServiceBusProcessorOptions
                    {
                        MaxConcurrentCalls = _options.Subscription.MaxConcurrentCalls,
                        MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(_options.Subscription.MaxAutoLockRenewalDurationMinutes),
                        AutoCompleteMessages = _options.Subscription.AutoComplete
                    });

                    _receivers.Add(binding.Exchange, processor);

                    processor.ProcessMessageAsync += MessageHandler;
                    processor.ProcessErrorAsync += ErrorHandler;
                    await processor.StartProcessingAsync();
                }

                try
                {
                    await _adminClient.CreateRuleAsync(binding.Exchange, subscriptionName, new CreateRuleOptions
                    {
                        Filter = new CorrelationRuleFilter(binding.Topic),
                        Name = $"{binding.Exchange}_{ GetSubscriptionName(binding.Topic)}"
                    });
                }
                catch (ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
                { }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task<ServiceBusSender> CreateTopicForSubscriptionAsync(string exchange)
        {            
            if (!_senders.TryGetValue(exchange, out var sender))
            {
                if (!await _adminClient.TopicExistsAsync(exchange))
                {
                    _logger?.LogDebug("Topic not exist, creating Topic {Topic}", exchange);

                    try
                    {
                        await _adminClient.CreateTopicAsync(exchange);

                        _logger?.LogDebug("Topic {Topic} created successfully", exchange);
                    }
                    catch (ServiceBusException e) when (e.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
                    {
                        _logger?.LogDebug("Topic {Topic} already exist", exchange);
                    }
                }
                sender = _sbClient.CreateSender(exchange);
                _senders.Add(exchange, sender);
            }
            return sender;
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var topic = args.Message.CorrelationId;
            var exchange = args.Message.ApplicationProperties["exchange"] as string;
            try
            {
                if (exchange == null)
                {
                    _logger?.LogWarning("Topic not found for message {MessageId}", args.Message.MessageId);
                }
                else
                {

                    var body = args.Message.Body;
                    _logger?.LogDebug("Message Dispatched {Exchange} {Topic}", exchange, topic);
                    
                    await _subscriptionManager.HandleMessage(exchange, topic, body.ToMemory(), _serializer).ConfigureAwait(false);
                }

                if (!_options.Subscription.AutoComplete)
                {
                    // complete the message. messages is deleted from the subscription. 
                    await args.CompleteMessageAsync(args.Message);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);

                if (!_options.Subscription.AutoComplete)
                {
                    await args.DeadLetterMessageAsync(args.Message);
                }
            }
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {            
            _logger?.LogError(args.Exception, args.Exception.Message);

            return Task.CompletedTask;
        }
   
        private string GetSubscriptionName(string topic)
        {
            return topic.Replace(':', '-');
        }
    }
}
