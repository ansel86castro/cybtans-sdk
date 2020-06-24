using Cybtans.Entities;
using Cybtans.Entities.EventLog;
using Cybtans.Messaging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityEventHandlerServiceCollectionExtensions
    {
        public static void SubscribeHandlerForEvents<TEntity, THandler>(this IMessageSubscriptionManager subscriptionManager, string exchange)
            where THandler : IEntityEventsHandler<TEntity>
        {
            subscriptionManager.RegisterBinding<EntityCreated<TEntity>>(exchange);
            subscriptionManager.RegisterBinding<EntityUpdated<TEntity>>(exchange);
            subscriptionManager.RegisterBinding<EntityDeleted<TEntity>>(exchange);

            subscriptionManager.Subscribe<EntityCreated<TEntity>, THandler>();
            subscriptionManager.Subscribe<EntityUpdated<TEntity>, THandler>();
            subscriptionManager.Subscribe<EntityDeleted<TEntity>, THandler>();
        }

        public static void SubscribeForEvents<TEntity, TKey>(this IMessageSubscriptionManager subscriptionManager, string exchange)
            where TEntity : IEntity<TKey>
        {
            subscriptionManager.SubscribeHandlerForEvents<TEntity, EntityEventsHandler<TEntity, TKey>>(exchange);
        }
    }
}
