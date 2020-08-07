using Cybtans.Entities;
using Cybtans.Entities.EventLog;
using Cybtans.Messaging;

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

        public static void SubscribeForEvents<TEntity, TKey>(this IMessageSubscriptionManager subscriptionManager, string exchange)
            where TEntity : IEntity<TKey>
        {
            subscriptionManager.SubscribeHandlerForEvents<TEntity, EntityEventsHandler<TEntity, TKey>>(exchange);
        }
    }
}
