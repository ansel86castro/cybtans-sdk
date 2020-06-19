using Cybtans.Messaging;

namespace Cybtans.Entities.EventLog
{
    public interface IEntityEventsHandler<T> :
        IMessageHandler<EntityCreated<T>>,
        IMessageHandler<EntityUpdated<T>>,
        IMessageHandler<EntityDeleted<T>>
    {

    }
}
