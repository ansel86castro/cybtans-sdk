using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Messaging.RabbitMQ
{
    public class RabbitBroadCastService : IBroadcastService
    {
        private readonly BroadcastServiceOptions options;
        private readonly BroadcastSubscriptionManager _subscriptionManager;
        private readonly RabbitMessageQueue _queue;

        public RabbitBroadCastService(
            BroadcastServiceOptions options,
            IConnectionFactory connectionFactory,
            BroadcastSubscriptionManager subscriptionManager, 
            ILoggerFactory loggerFactory)
        {
            this.options = options;
            _subscriptionManager = subscriptionManager;
            var queueOptions = new RabbitMessageQueueOptions
            {
                 Hostname = options.Hostname,
                 RetryCount = options.RetryCount,
                  Exchange = new ExchangeConfig
                  {
                       Name = options.Exchange
                  },
                  Queue = new QueueConfig
                  {
                      Name = null,
                      Durable = false,
                      AutoDelete = true,
                      Exclusive = true
                  }
            };

            _queue = new RabbitMessageQueue(connectionFactory, 
                _subscriptionManager.messageSubscriptionManager, 
                queueOptions, 
                loggerFactory.CreateLogger<RabbitMessageQueue>());
        }

        public IBroadcastSubscriptionManager Subscriptions => _subscriptionManager;

        public void Dispose()
        {
            _queue.Dispose();
        }

        public Task Publish(byte[] bytes, string channel)
        {
            return _queue.Publish(bytes, options.Exchange, channel);
        }

        public Task Publish(object message, string channel = null)
        {
            return _queue.Publish(message, options.Exchange, channel);
        }

        public void Start()
        {
            _queue.Start();
        }
    }
}
