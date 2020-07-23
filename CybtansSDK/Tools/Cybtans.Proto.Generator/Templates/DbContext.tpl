using Cybtans.Entities.EntityFrameworkCore;
using Cybtans.Entities.EventLog;
using Microsoft.EntityFrameworkCore;

namespace @{SERVICE}.Domain.EntityFramework
{
    public class @{SERVICE}Context : DbContext, IEntityEventLogContext
    {
        public @{SERVICE}Context()
        {
        }

        public @{SERVICE}Context(DbContextOptions<@{SERVICE}Context> options)
            : base(options)
        {
        }       

        public DbSet<EntityEventLog> EntityEventLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlite("Data Source=@{SERVICE};Mode=Memory;Cache=Shared");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
