using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Entities
{
    public class EntityException : Exception
    {
        public EntityException() { }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class EntityNotFoundException : EntityException
    {
        public IEnumerable<object> Entities { get; set; }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string message, IEnumerable<object> entities)
            : base(message)
        {
            this.Entities = entities;
        }
    }

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
    }
}
