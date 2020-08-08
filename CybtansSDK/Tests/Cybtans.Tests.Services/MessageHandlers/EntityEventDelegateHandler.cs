using System;

namespace Cybtans.Tests.Services
{
    public class EntityEventDelegateHandler<T>
    {
        public EventHandler<EntityEventArg> OnCreated { get; set; }
        public EventHandler<EntityEventArg> OnUpdated { get; set; }
        public EventHandler<EntityEventArg> OnDeleted { get; set; }
    }
}
