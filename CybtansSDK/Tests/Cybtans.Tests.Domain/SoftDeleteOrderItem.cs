using Cybtans.Entities;
using System;

namespace Cybtans.Tests.Domain
{
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
