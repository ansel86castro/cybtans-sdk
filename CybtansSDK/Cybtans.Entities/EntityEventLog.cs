using System;

namespace Cybtans.Entities
{
    public class EntityEventLog:Entity<Guid>
    {        
        public DateTime CreateTime { get; set; }

        public EventStateEnum State { get; set; }

        public string EntityEventType { get; set; }

        public string Exchange { get; set; }

        public string Topic { get; set; }

        public byte[] Data { get; set; }

        public string ErrorMessage { get; set; }

    }

}
