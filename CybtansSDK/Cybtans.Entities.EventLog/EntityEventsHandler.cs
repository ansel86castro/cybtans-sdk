using System.Threading.Tasks;

namespace Cybtans.Entities.EventLog
{
    public class EntityEventsHandler<T, TKey> : IEntityEventsHandler<T>
    {
        IRepository<T, TKey> _repository;

        public EntityEventsHandler(IRepository<T, TKey> repository)
        {
            _repository = repository;
        }

        public virtual async Task HandleMessage(EntityCreated<T> message)
        {
            _repository.Add(message.Value);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public virtual async Task HandleMessage(EntityUpdated<T> message)
        {
            _repository.Update(message.NewValue);
            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public virtual async Task HandleMessage(EntityDeleted<T> message)
        {
            _repository.Remove(message.Value);
            await _repository.UnitOfWork.SaveChangesAsync();
        }
    }
}
