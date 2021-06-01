using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cybtans.Tests.Domain
{
    [GenerateMessage(Service = ServiceType.Interface)]
    public class Order : DomainTenantEntity<Guid>
    {
        [EventData]
        public string Description { get; set; }

        [EventData]
        public Guid CustomerId { get; set; }

        [EventData]
        public int OrderStateId { get; set; }

        [EventData]
        public OrderTypeEnum OrderType { get; set; }


        [EventData]
        public virtual OrderState OrderState { get; set; }

        [Description("Customer")]
        public virtual Customer Customer { get; set; }

        [EventData]
        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }
}
