using Cybtans.Entities.EntiyFrameworkCore;
using Cybtans.Entities.EventLog;
using Cybtans.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public interface IEventPublisherBuilder
    {
        IEntityEventPublisher Create(IServiceProvider provider);
    }

    public class EventPublisherBuilder<TContext> : IEventPublisherBuilder
        where TContext : DbContext, IEntityEventLogContext
    {
        public IEntityEventPublisher Create(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<TContext>();
            var messageQueue = provider.GetRequiredService<IMessageQueue>();
            var uow = new EfUnitOfWork(context);
            return new EntityEventPublisher(
                messageQueue,
                uow.CreateRepository<EntityEventLog, Guid>(),
                provider.GetService<ILogger<EntityEventPublisher>>());
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork>((srvProvicer) =>
            {
                var eventPublisher = srvProvicer.GetService<IEntityEventPublisher>();
               return new EfUnitOfWork(srvProvicer.GetRequiredService<TDbContext>(), eventPublisher);
            });
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

            return services;
        }

        public static IServiceCollection AddDbContextEventPublisher<TContext>(this IServiceCollection services)
            where TContext:DbContext, IEntityEventLogContext
        {           
            services.AddScoped<IEntityEventPublisher>(provider =>
            {
                var context = provider.GetRequiredService<TContext>();
                var messageQueue = provider.GetRequiredService<IMessageQueue>();
                var uow = new EfUnitOfWork(context);
                return new EntityEventPublisher(
                    messageQueue,
                    uow.CreateRepository<EntityEventLog, Guid>(),
                    provider.GetService<ILogger<EntityEventPublisher>>());
            });
            return services;
        }       
    }
}
