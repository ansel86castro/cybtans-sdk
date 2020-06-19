using System;

namespace Cybtans.Entities
{

    public class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }

    public class AuditableEntity<T> : Entity<T>, IAuditableEntity        
    {
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Creator { get; set; }
    }

    public class TenantEntity<TKey> : AuditableEntity<TKey>
    {
        public Guid? TenantId { get; set; }
    }
}
