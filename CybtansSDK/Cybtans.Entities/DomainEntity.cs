using System;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Entities
{
    public class DomainEntity: IDomainEntity
    {
        private List<EntityEvent> _domainEvents;        

        public void AddEntityEvent(EntityEvent domainEvent)
        {
            _domainEvents ??= new List<EntityEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void ClearEntityEvents(EventStateEnum? eventState)
        {
            if (eventState == null)
            {
                _domainEvents?.Clear();
            }
            else
            {
                _domainEvents = _domainEvents.Where(x => x.State != eventState).ToList();
            }
        }

        public IReadOnlyCollection<EntityEvent> GetDomainEvents()
        {
            return _domainEvents?.AsReadOnly();
        }

        public bool HasEntityEvents()
        {
            return _domainEvents != null && _domainEvents.Count > 0;
        }

        public void RemoveEntityEvent(EntityEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public virtual void AddCreatedEvent() 
        {
            var type = typeof(EntityCreated<>).MakeGenericType(GetType());
            AddEntityEvent((EntityEvent)Activator.CreateInstance(type, this));
        }

        public virtual void AddUpdatedEvent(IDomainEntity oldValue) 
        {            
            var type = typeof(EntityUpdated<>).MakeGenericType(GetType());
            AddEntityEvent((EntityEvent)Activator.CreateInstance(type, this, oldValue));
        }

        public virtual void AddDeletedEvent()         
        {
            var type = typeof(EntityDeleted<>).MakeGenericType(GetType());
            AddEntityEvent((EntityEvent)Activator.CreateInstance(type, this));
        }
    }

    public class DomainAuditableEntity : DomainEntity, IAuditableEntity
    {        
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
        
        public string Creator { get; set; }
    }

    public class DomainEntity<TKey> : DomainAuditableEntity, IDomainEntity<TKey>
    {
        [EventData]
        public TKey Id { get; set; }
    }

    public class DomainTenantEntity<TKey> : DomainEntity<TKey>
    {
        [EventData]
        public Guid? TenantId { get; set; }
    }


}
