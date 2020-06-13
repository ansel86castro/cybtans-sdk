using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Entities
{
    public class EntityEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public EventStateEnum State { get; set; } = EventStateEnum.NotPublished;
    }

    public enum EventStateEnum
    {
        NotPublished,       
        Published        
    }

    public interface IEntityEventLogRegistry
    {
        void Add(EntityEvent entityEvent);

        ValueTask<EntityEvent> Get(Guid id);

        ValueTask<EntityEvent> UpdateState(Guid id, EventStateEnum state);

        IEnumerable<EntityEvent> GetAll();

        void Remove(Guid id);

        Task SaveChangesAsyc();
        
    }
    
}
