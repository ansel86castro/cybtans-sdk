using System;

namespace Cybtans.Entities
{
    public interface ITenantEntity:IEntity
    {
        Guid? TenantId { get; set; }
    }
}
