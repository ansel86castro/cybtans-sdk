#nullable enable

namespace Cybtans.Messaging.RabbitMQ
{
    public class RabbitMessageQueueOptions
    {
        public int RetryCount { get; set; } = 5;

        public string ExchangeType { get; set; } = "topic";

        public bool Durable { get; set; } = true;

        public bool PublisherAcknowledge { get; set; } = true;

        public string? BradcastAlternateExchange { get; set; }

        public string? QueueName { get; set; }

        public bool Exclusive { get; set; } = true;
    }
}
