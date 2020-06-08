using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Entities.EntityFrameworkCore
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

    public class EntityEventLogContext:DbContext
    {
        public EntityEventLogContext(DbContextOptions<EntityEventLogContext> options)
        : base(options)
        {

        }

        public DbSet<EntityEventLog> Events { get; set; }
    }
}
