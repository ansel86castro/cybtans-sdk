﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities.EntityFrameworkCore
{

    public class EfUnitOfWork : IDbContextUnitOfWork
    {
        private DbContext _context;        

        public EfUnitOfWork(DbContext context, IEntityEventPublisher? eventPublisher)
        {
            _context = context;
            EventPublisher = eventPublisher;
        }

        public EfUnitOfWork(DbContext context) : this(context, null)
        {

        }

        public DbContext Context => _context;

        public IEntityEventPublisher? EventPublisher { get; set; }

        public virtual DbContext GetContext(ReadConsistency consistency)
        {
            return _context;
        }

        public virtual IRepository<T, TKey> CreateRepository<T, TKey>()
            where T : class
        {
            return new EfRepository<T, TKey>(this);
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

       


        public virtual int SaveChanges()
        {
            //ApplySoftDelete();
            return SaveChangesInternal(2);         
        }

     
        public async Task<int> SaveChangesAsync()
        {                       
            if (EventPublisher == null)
            {
               // ApplySoftDelete();
                return await SaveChangesAsyncInternal(2).ConfigureAwait(false);
            }
            else
            {
                var entities = GetDomainEntries();
                //ApplySoftDelete();
                var rows = await SaveChangesAsyncInternal(2).ConfigureAwait(false);
                if (rows > 0)
                {
                    var events = entities.SelectMany(x => x.GetDomainEvents())
                        .Where(x => x.State == EventStateEnum.NotPublished)
                        .ToList();

                    await EventPublisher.PublishAll(events).ConfigureAwait(false);

                    foreach (var entry in entities)
                    {
                        entry.ClearEntityEvents(EventStateEnum.Published);
                    }
                }

                return rows;
            }
        }

        private List<IDomainEntity> GetDomainEntries()
        {
            var entries = _context.ChangeTracker.Entries<IDomainEntity>();
            foreach (var entry in entries)
            {
                if (!entry.Entity.HasEntityEvents())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.AddCreatedEvent();
                            break;
                        case EntityState.Modified:
                            entry.Entity.AddUpdatedEvent(entry.OriginalValues.ToObject() as IDomainEntity);
                            break;
                        case EntityState.Deleted:
                            entry.Entity.AddDeletedEvent();
                            break;
                    }
                }
            }

            return entries.Where(x => x.Entity.HasEntityEvents()).Select(x=>x.Entity).ToList();
        }

        //private void  ApplySoftDelete()
        //{
        //    foreach (var entry in _context.ChangeTracker.Entries<ISoftDelete>().Where(x=>x.State == EntityState.Deleted))
        //    {
        //        entry.State = EntityState.Unchanged;
        //        entry.Entity.IsDeleted = true;
        //    }
        //}

        private async Task<int> SaveChangesAsyncInternal(int retryCount)
        {           
            retryCount--;
            if (retryCount == 0)
                throw new EntityNotFoundException("Can not save changes");

            try
            {
                return await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Update original values from the database 
                foreach (var entry in ex.Entries)
                {
                    var dbValues = entry.GetDatabaseValues();
                    if (dbValues != null)
                    {
                        entry.OriginalValues.SetValues(dbValues);
                    }
                }

                return await SaveChangesAsyncInternal(retryCount).ConfigureAwait(false);
            }
        }

        private int SaveChangesInternal(int retryCount)
        {
            retryCount--;

            if (retryCount == 0)
                throw new EntityNotFoundException("Can not save changes");

            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Update original values from the database 
                foreach (var entry in ex.Entries)
                {
                    var dbValues = entry.GetDatabaseValues();
                    if (dbValues != null)
                    {
                        entry.OriginalValues.SetValues(dbValues);
                    }
                }

                return SaveChangesInternal(retryCount);
            }
        }     
    }
}
