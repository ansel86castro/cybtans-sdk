using System.Collections.Generic;

namespace Cybtans.Entities
{
    public interface IEntity
    {
        IReadOnlyCollection<EntityEvent> GetDomainEvents();

        void AddEntityEvent(EntityEvent domainEvent);

        void RemoveEntityEvent(EntityEvent domainEvent);

        void ClearEntityEvents(EventStateEnum? type);

        public bool HasEntityEvents();
    }
   
}
