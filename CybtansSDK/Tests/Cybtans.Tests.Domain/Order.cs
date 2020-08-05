using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Default)]
    public class Order:DomainTenantEntity<Guid>
    {
        public string Description { get; set; }        

        public Guid CustomerId { get; set; }

        public int OrderStateId { get; set; }

        public virtual OrderState OrderState { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }
}
