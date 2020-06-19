using System.Collections.Generic;

namespace Cybtans.Entities
{
    public interface IDomainEntity:IEntity
    {
        IReadOnlyCollection<EntityEvent> GetDomainEvents();

        void AddEntityEvent(EntityEvent domainEvent);

        void RemoveEntityEvent(EntityEvent domainEvent);

        void ClearEntityEvents(EventStateEnum? type);

        public bool HasEntityEvents();
       
        void AddCreatedEvent();
        
        void AddUpdatedEvent(IDomainEntity oldValue);
        
        void AddDeletedEvent();
    }

    public interface IDomainEntity<T> : IDomainEntity, IEntity<T>
    {
        
    }
}
