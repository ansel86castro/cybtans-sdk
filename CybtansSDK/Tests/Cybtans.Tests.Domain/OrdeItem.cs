using Cybtans.Entities;
using System;

namespace Cybtans.Tests.Domain
{
    [GenerateMessage]
    public class OrderItem : Entity<Guid>
    {
        [EventData]
        public string ProductName { get; set; }

        [EventData]
        public float Price { get; set; }

        [EventData]
        public float? Discount { get; set; }

        public Guid OrderId { get; set; }

        [MessageExcluded]
        public virtual Order Order { get; set; }
    }
}
