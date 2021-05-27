using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Cybtans.Entities
{
    [Serializable]
    public class EntityException : Exception
    {
        public EntityException() { }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    public sealed class EntityNotFoundException : EntityException
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    [Serializable]
    public class EntityEventIntegrationException : EntityException
    {
        public IEnumerable<EntityEvent> Events { get; }

        public EntityEventIntegrationException(string message, IEnumerable<EntityEvent> events) : base(message)
        {
            Events = events;
        }

        public EntityEventIntegrationException(string message, IEnumerable<EntityEvent> events, Exception innerException) : base(message, innerException)
        {
            Events = events;
        }

        protected EntityEventIntegrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
