using Cybtans.Entities.EntiyFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Entities.EntityFrameworkCore
{ 
    public class EntityEventLogContext:DbContext
    {
        public EntityEventLogContext(DbContextOptions<EntityEventLogContext> options)
        : base(options)
        {

        }

        public DbSet<EntityEventLog> Events { get; set; }
    }
   
}
