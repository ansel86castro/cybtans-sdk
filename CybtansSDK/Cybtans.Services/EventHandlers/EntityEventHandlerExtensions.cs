using Cybtans.Entities;
using Cybtans.Messaging;
using Cybtans.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityEventHandlerServiceCollectionExtensions
    {
        public static void SubscribeHandlerForEvents<TEntity, THandler>(this IMessageSubscriptionManager subscriptionManager, string exchange, string entityTypeName)
          where THandler : IEntityEventsHandler<TEntity>
        {
            subscriptionManager.RegisterBinding<EntityCreated<TEntity>>(exchange, $"{entityTypeName}:EntityCreated");
            subscriptionManager.RegisterBinding<EntityUpdated<TEntity>>(exchange, $"{entityTypeName}:EntityUpdated");
            subscriptionManager.RegisterBinding<EntityDeleted<TEntity>>(exchange, $"{entityTypeName}:EntityDeleted");

            subscriptionManager.Subscribe<EntityCreated<TEntity>, THandler>();
            subscriptionManager.Subscribe<EntityUpdated<TEntity>, THandler>();
            subscriptionManager.Subscribe<EntityDeleted<TEntity>, THandler>();
        }

        public static void SubscribeHandlerForEvents<TEntity, THandler>(this IMessageSubscriptionManager subscriptionManager, string exchange)
            where THandler : IEntityEventsHandler<TEntity>
        {
          
             SubscribeHandlerForEvents<TEntity, THandler>(subscriptionManager, exchange, typeof(TEntity).Name);
        }       

        public static void SubscribeForEvents<TEntity>(this IMessageSubscriptionManager subscriptionManager, string exchange)            
        {
            SubscribeHandlerForEvents<TEntity, EntityEventsHandler<TEntity>>(subscriptionManager, exchange);
        }

        public static void SubscribeForEvents<TEntity>(this IMessageSubscriptionManager subscriptionManager, string exchange, string entityName)            
        {
            SubscribeHandlerForEvents<TEntity, EntityEventsHandler<TEntity>>(subscriptionManager, exchange, entityName);
        }

        public static void SubscribeForEvents<TEntity, TEvent>(this IMessageSubscriptionManager subscriptionManager, string exchange)           
        {
            SubscribeHandlerForEvents<TEvent, EntityEventsHandler<TEntity, TEvent>>(subscriptionManager, exchange, typeof(TEvent).Name);
        }

        public static void SubscribeForEvents<TEntity, TEvent>(this IMessageSubscriptionManager subscriptionManager, string exchange, string eventName)          
        {
            SubscribeHandlerForEvents<TEvent, EntityEventsHandler<TEntity, TEvent>>(subscriptionManager, exchange, eventName);
        }

        public static void AddMessageHandler<TEntity>(this IServiceCollection services)
        {
            services.AddScoped<EntityEventsHandler<TEntity>>();
        }
     
        public static void AddMessageHandler<TEntity, TEvent>(this IServiceCollection services)
        {
            services.AddScoped<EntityEventsHandler<TEntity, TEvent>>();
        }


    }
}
