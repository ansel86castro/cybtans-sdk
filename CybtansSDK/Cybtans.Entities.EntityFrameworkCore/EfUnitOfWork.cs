using Cybtans.Entities.EntiyFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private DbContext _context;        

        public EfUnitOfWork(DbContext context, IEntityEventPublisher? eventPublisher = null)
        {
            _context = context;
            EventPublisher = eventPublisher;
        }        

        public IEntityEventPublisher? EventPublisher { get; set; }

        public IRepository<T, TKey> CreateRepository<T, TKey>()
            where T : class
        {
            return new EfRepository<T, TKey>(_context, this);
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

        public async Task<T> ExecuteResilientTransacion<T>(Func<Task<T>> action)
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = _context.Database.BeginTransaction();
                var result = await action();
                transaction.Commit();
                return result;
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

                    if (retryCount == 0)
                        throw new EntityNotFoundException("Can not save changes", ex.Entries.Select(x => x.Entity));

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

        private async Task<int> SaveChangesAsyncInternal()
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

        public Task<int> SaveChangesAsync()
        {
            if (EventPublisher == null)
            {
                return SaveChangesAsyncInternal();
            }

            return ExecuteResilientTransacion(async () =>
            {
                var rows = await SaveChangesAsyncInternal();

                if (rows > 0)
                {
                    var entities = _context.ChangeTracker.Entries<IEntity>()
                                     .Where(x => x.Entity.HasEntityEvents())
                                     .Select(x => x.Entity);

                    var events = entities.SelectMany(x => x.GetDomainEvents()).Where(x => x.State == EventStateEnum.NotPublished).ToList();

                    await EventPublisher.PublishAll(events);

                    foreach (var item in entities)
                    {
                        item.ClearEntityEvents(EventStateEnum.NotPublished);
                    }
                }
                return rows;
            });
        }
    }
}
