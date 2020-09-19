using Cybtans.Entities;
using Cybtans.Services;
using Cybtans.Tests.Domain;
using System.Threading.Tasks;

namespace Cybtans.Tests.Services
{

    public class OrderMessageHandler : IEntityEventsHandler<Order>
    {
        EntityEventDelegateHandler<OrderMessageHandler> _options;

        public OrderMessageHandler(EntityEventDelegateHandler<OrderMessageHandler> options)
        {
            _options = options;
        }

        public Task HandleMessage(EntityCreated<Order> message)
        {
            _options.OnCreated?.Invoke(this, message);
            return Task.CompletedTask;
        }

        public Task HandleMessage(EntityUpdated<Order> message)
        {
            _options.OnUpdated?.Invoke(this, message);
            return Task.CompletedTask;
        }

        public Task HandleMessage(EntityDeleted<Order> message)
        {
            _options.OnDeleted?.Invoke(this, message);
            return Task.CompletedTask;
        }
    }
}
