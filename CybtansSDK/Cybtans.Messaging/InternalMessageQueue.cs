using Cybtans.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Messaging
{
    public class InternalMessageQueue : IMessageQueue
    {        
        private readonly MessageSubscriptionManager _subscriptionManager;

        public InternalMessageQueue(MessageSubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        public void Dispose()
        {
           
        }

        public BindingInfo? GetBinding(Type type, string topic)
        {
            return _subscriptionManager.GetBindingForType(type, null, topic);
        }

        public Task Publish(byte[] bytes, string exchange, string topic)
        {
            return _subscriptionManager.HandleMessage(exchange, topic, bytes);
        }

        public async Task Publish(object message, string? exchange, string? topic)
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

            var data = BinaryConvert.Serialize(message);
            await _subscriptionManager.HandleMessage(exchange, topic, data).ConfigureAwait(false);
        }

        public void RegisterBinding<T>(string exchage, string? topic = null)
        {
            _subscriptionManager.RegisterBinding<T>(exchage, topic);
        }

        public virtual void Start()
        {
            
        }

        public void Subscribe<TMessage, THandler>(string? exchange = null, string? topic = null) where THandler : IMessageHandler<TMessage>
        {
            _subscriptionManager.Subscribe<TMessage, THandler>(exchange, topic);
        }

        public void Subscribe<TMessage>(IMessageHandler<TMessage> handler, string? exchange = null, string? topic = null)
        {
            _subscriptionManager.Subscribe<TMessage>(handler, exchange, topic);
        }

        public void Unsubscribe<TMessage, THandler>(string? exchange = null, string? topic = null) where THandler : IMessageHandler<TMessage>
        {
            _subscriptionManager.Unsubscribe<TMessage, THandler>(exchange, topic);
        }
    }
}
