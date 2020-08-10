using Cybtans.Entities;
using Cybtans.Messaging;

namespace Cybtans.Services
{
    public interface IEntityEventsHandler<T> :
        IMessageHandler<EntityCreated<T>>,
        IMessageHandler<EntityUpdated<T>>,
        IMessageHandler<EntityDeleted<T>>
    {

    }
}
