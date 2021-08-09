using System;

namespace Cybtans.Messaging.AzureServiceBus
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; }

        public TopicOptions Topic { get; set; }

        public SubscriptionOptions Subscription { get; set; }
    }

    public class TopicOptions
    {
        public string Name { get; set; }

        public bool Create { get; set; }
    }

    public class SubscriptionOptions 
    {
        public string Name { get; set; }

        public bool Create { get; set; }

        public int MaxDeliveryCount { get; set; } = 2000;

        public int MaxConcurrentCalls { get; set; } = 4;

        public int MaxAutoLockRenewalDurationMinutes { get; set; } = 5;

        public bool AutoComplete { get; set; }
    }
}
