using Cybtans.Entities;
using System;
using System.Collections.Generic;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Default)]
    public class SoftDeleteOrder : DomainTenantEntity<Guid>, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<SoftDeleteOrderItem> Items { get; set; } = new HashSet<SoftDeleteOrderItem>();
    }
}
