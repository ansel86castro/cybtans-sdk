using System;

namespace Cybtans.Entities
{
    public class EntityEventLog
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        public EventStateEnum State { get; set; }

        public string EntityEventType { get; set; }

        public string Exchange { get; set; }

        public string Topic { get; set; }

        public byte[] Data { get; set; }
    }
    
}
