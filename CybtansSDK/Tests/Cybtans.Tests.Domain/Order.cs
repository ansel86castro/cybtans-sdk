using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Interface)]
    public class Order:DomainTenantEntity<Guid>
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
        
        public virtual Customer Customer { get; set; }

        [EventData]
        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }

    [GenerateMessage]
    public enum OrderTypeEnum
    {
        Default,
        Normal,
        Shipping
    }

    [GenerateMessage(Service = ServiceType.Default)]
    public class SoftDeleteOrder : DomainTenantEntity<Guid>, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<SoftDeleteOrderItem> Items { get; set; } = new HashSet<SoftDeleteOrderItem>();
    }

    [GenerateMessage]
    public class SoftDeleteOrderItem : DomainTenantEntity<Guid>, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public Guid SoftDeleteOrderId { get; set; }

        [MessageExcluded]
        public virtual SoftDeleteOrder SoftDeleteOrder { get; set; }
    }
}
