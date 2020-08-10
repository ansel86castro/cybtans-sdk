using Cybtans.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cybtans.Services
{
    public class EntityEventsHandler<T> : IEntityEventsHandler<T>
    {
        private readonly IRepository<T> _repository;
        private readonly ILogger<EntityEventsHandler<T>> _logger;

        public EntityEventsHandler(IRepository<T> repository, ILogger<EntityEventsHandler<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public virtual async Task HandleMessage(EntityCreated<T> message)
        {
            _repository.Add(message.Value);
            await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            _logger.LogDebug($"IntegrationEvent: Entity {typeof(T).Name} Created");
        }

        public virtual async Task HandleMessage(EntityUpdated<T> message)
        {
            _repository.Update(message.NewValue);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogDebug($"IntegrationEvent: Entity {typeof(T).Name} Updated");
            }
            catch (EntityNotFoundException)
            {
                _repository.Remove(message.NewValue);

                EntityCreated<T> created = new EntityCreated<T>(message.NewValue);
                await HandleMessage(created);
            }
        }

        public virtual async Task HandleMessage(EntityDeleted<T> message)
        {
            _repository.Remove(message.Value);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogDebug($"IntegrationEvent: Entity {typeof(T).Name} Removed");
            }
            catch (EntityNotFoundException)
            {
                //Do nothing
                _logger.LogWarning($"IntegrationEvent: Entity {typeof(T).Name} can not be removed beacause is not found");
            }
        }
    }   

}
