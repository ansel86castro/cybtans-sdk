#nullable enable

using System.Collections;
using System.Reflection;

namespace Cybtans.Messaging.RabbitMQ
{    
    public class RabbitMessageQueueOptions
    {
        public string Hostname { get; set; } = "localhost";

        public int RetryCount { get; set; } = 5;

        public string ExchangeType { get; set; } = "topic";

        public ExchangeConfig Exchange { get; set; } = new ExchangeConfig();

        public QueueConfig Queue { get; set; } = new QueueConfig();        
    }

    public class ExchangeConfig
    {
        public string Type { get; set; } = "topic";

        public string? Name { get; set; }

        public bool PublisherAcknowledge { get; set; } = false;

        public string? BradcastAlternateExchange { get; set; }

        public bool PersistMessages { get; set; } = true;

        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; } = false;
    }

    public class QueueConfig
    {
        public string? Name { get; set; }

        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; } = true;

        public bool Exclusive { get; set; } = true;
    }
}
