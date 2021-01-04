using Microsoft.Extensions.Logging;
using System;

namespace Cybtans.Messaging
{
    public class BroadcastServiceOptions
    {
        public string Hostname { get; set; } = "localhost";

        public string Exchange { get; set; }        

        public int RetryCount { get; set; } = 5;
    }

    public class BroadcastSubscriptionManager : IBroadcastSubscriptionManager
    {
        private BroadcastServiceOptions _options;
        MessageSubscriptionManager _subscriptionManager;

        public BroadcastSubscriptionManager(
            BroadcastServiceOptions options,
            IServiceProvider? provider = null, 
            ILoggerFactory? loggerFactory = null)
        {
            _options = options;
            _subscriptionManager = new MessageSubscriptionManager(provider, options.Exchange, loggerFactory?.CreateLogger<MessageSubscriptionManager>());
        }

        public MessageSubscriptionManager messageSubscriptionManager => _subscriptionManager;

        public void Subscribe<TMessage, THandler>(string channel = null) where THandler : IMessageHandler<TMessage>
        {
            _subscriptionManager.Subscribe<TMessage, THandler>(_options.Exchange, channel);
        }

        public void Subscribe<TMessage>(IMessageHandler<TMessage> handler, string channel = null)
        {
            _subscriptionManager.Subscribe<TMessage>(handler, _options.Exchange, channel);
        }

        public void Unsubscribe<TMessage>(string? channel = null)
        {
            _subscriptionManager.Unsubscribe<TMessage>(channel);
        }
    }
}
