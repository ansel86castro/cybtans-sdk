using Cybtans.Entities;
using System;

namespace Cybtans.Test.Domain
{
    [GenerateMessage]
    public class OrderItem: Entity<Guid>
    {
        public string ProductName { get; set; }

        public float Price { get; set; }

        public float Discount { get; set; }

        public Guid OrderId { get; set; }

        [MessageExcluded]
        public virtual Order Order { get; set; }
    }
}
