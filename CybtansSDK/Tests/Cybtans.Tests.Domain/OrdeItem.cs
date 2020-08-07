using Cybtans.Entities;
using System;

namespace Cybtans.Test.Domain
{
    [GenerateMessage]
    public class OrderItem: Entity<Guid>
    {
        [EventMember]
        public string ProductName { get; set; }

        [EventMember]
        public float Price { get; set; }

        [EventMember]
        public float Discount { get; set; }

        public Guid OrderId { get; set; }

        [MessageExcluded]
        public virtual Order Order { get; set; }
    }
}
