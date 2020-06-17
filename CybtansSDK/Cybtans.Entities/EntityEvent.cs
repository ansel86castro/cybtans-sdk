using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Entities
{
    public class EntityEvent
    {
        public Guid Id { get; set; }        

        public DateTime CreateTime { get; set; } = DateTime.Now;        

        public EventStateEnum State { get; set; } = EventStateEnum.NotPublished;
    }

    public class EntityCreated<T>:EntityEvent
    {
        public EntityCreated()
        {
        }

        public EntityCreated(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }

    public class EntityUpdated<T> : EntityEvent
    {
        public EntityUpdated()
        {

        }

        public EntityUpdated(T newValue, T oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        public T NewValue { get; private set; }

        public T OldValue { get; private set; }
    }

    public class EntityDeleted<T> : EntityEvent
    {
        public EntityDeleted()
        {

        }

        public EntityDeleted(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }



    public enum EventStateEnum
    {
        NotPublished,       
        Published        
    }
    
   
}
