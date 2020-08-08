using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Interface)]
    public class Order:DomainTenantEntity<Guid>
    {
        [EventMember]
        public string Description { get; set; }

        [EventMember]
        public Guid CustomerId { get; set; }

        [EventMember]
        public int OrderStateId { get; set; }

        [EventMember]
        public virtual OrderState OrderState { get; set; }
        
        public virtual Customer Customer { get; set; }

        [EventMember]
        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }
}
