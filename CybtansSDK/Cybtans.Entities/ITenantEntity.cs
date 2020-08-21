using System;

namespace Cybtans.Entities
{
    public interface ITenantEntity:IEntity
    {
        Guid? TenantId { get; set; }
    }

    public interface ISoftDelete : IEntity
    {
        bool IsDeleted { get; set; }
    }
}
