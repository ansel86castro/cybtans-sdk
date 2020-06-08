using System;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Entities
{

    public class Entity: IEntity
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
    }   

    public class AuditableEntity : Entity
    {
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }

    public class Entity<TKey>: AuditableEntity
    {
        public TKey Id { get; set; }
    }


}
