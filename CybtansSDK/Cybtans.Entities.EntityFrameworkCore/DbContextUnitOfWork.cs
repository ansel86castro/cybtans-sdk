using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntiyFrameworkCore
{
    public class DbContextUnitOfWork : IUnitOfWork
    {
        private DbContext _context;
        private IEntityEventPublisher _eventPublisher;

        public DbContextUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public DbContextUnitOfWork(DbContext context , IEntityEventPublisher eventPublisher) : this(context)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task ExecuteResilientTransacion(Func<Task> action)
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = _context.Database.BeginTransaction();
                await action();
                transaction.Commit();
            });
        }

        public int SaveChanges()
        {
            int retryCount = 2;            
            while (retryCount > 0)
            {
                try
                {
                    return _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryCount--;

                    if(retryCount == 0)
                        throw new EntityNotFoundException("Can not save changes", ex.Entries.Select(x=>x.Entity));

                    // Update original values from the database 
                    foreach (var entry in ex.Entries)
                    {
                        var dbValues = entry.GetDatabaseValues();
                        if (dbValues != null)
                        {
                            entry.OriginalValues.SetValues(dbValues);
                        }
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public async Task<int> SaveChangesAsync()
        {
            int retryCount = 2;         
            while (retryCount > 0)
            {
                try
                {
                    return await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryCount--;

                    if (retryCount == 0)
                        throw new EntityNotFoundException("Can not save changes", ex.Entries.Select(x => x.Entity));

                    // Update original values from the database 
                    foreach (var entry in ex.Entries)
                    {
                        var dbValues = await entry.GetDatabaseValuesAsync();
                        if (dbValues != null)
                        {
                            entry.OriginalValues.SetValues(dbValues);
                        }
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public Task SaveChangesAndPublishAsync()
        {
            if (_eventPublisher == null)
            {
                return SaveChangesAsync();
            }

            return ExecuteResilientTransacion(async () =>
            {
                await SaveChangesAsync();

                var entities = _context.ChangeTracker.Entries<IEntity>()
                                 .Where(x => x.Entity.HasEntityEvents())
                                 .Select(x => x.Entity);

                var events = entities.SelectMany(x => x.GetDomainEvents()).Where(x=>x.State == EventStateEnum.NotPublished);

                await _eventPublisher.PublishAll(events);

                foreach (var item in entities)
                {
                    item.ClearEntityEvents(EventStateEnum.NotPublished);
                }
            });
        }
    }
}
