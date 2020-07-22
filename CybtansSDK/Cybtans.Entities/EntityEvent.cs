using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Entities
{
    public interface ITopicProvider
    {
        public string Topic { get; }
    }

    public class EntityEvent: ITopicProvider
    {
        public Guid Id { get; set; }        

        public DateTime CreateTime { get; set; } = DateTime.Now;        

        public EventStateEnum State { get; set; } = EventStateEnum.NotPublished;

        public virtual string Topic => GetType().Name;
    }

    public class EntityCreated<T>:EntityEvent
    {
        public static readonly string TOPIC = $"{typeof(T).Name}:EntityCreated";

        public EntityCreated()
        {
        }

        public EntityCreated(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public override string Topic => TOPIC;
    }

    public class EntityUpdated<T> : EntityEvent
    {
        public static readonly string TOPIC = $"{typeof(T).Name}:EntityUpdated";

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

        public override string Topic => TOPIC;        
    }

    public class EntityDeleted<T> : EntityEvent
    {
        public static readonly string TOPIC = $"{typeof(T).Name}:EntityDeleted";

        public EntityDeleted()
        {

        }

        public EntityDeleted(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public override string Topic => TOPIC;
    }



    public enum EventStateEnum
    {
        NotPublished,       
        Published        
    }
    
   
}
