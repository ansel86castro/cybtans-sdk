using Cybtans.Entities;
using System;

namespace Cybtans.Tests.Services
{
    public class EntityEventArg : EventArgs
    {
        public EntityEvent Event { get; set; }

        public static implicit operator EntityEventArg(EntityEvent @event)
        {
            return new EntityEventArg { Event = @event };
        }
    }
}
