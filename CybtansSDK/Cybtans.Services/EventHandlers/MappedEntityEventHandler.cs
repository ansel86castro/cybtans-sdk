using AutoMapper;
using Cybtans.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cybtans.Services
{
    public class EntityEventsHandler<TEntity, TEvent> : IEntityEventsHandler<TEvent>
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EntityEventsHandler<TEntity, TEvent>> _logger;

        public EntityEventsHandler(IRepository<TEntity> repository, IMapper mapper, ILogger<EntityEventsHandler<TEntity, TEvent>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public virtual async Task HandleMessage(EntityCreated<TEvent> message)
        {
            var entity = _mapper.Map<TEvent, TEntity>(message.Value);
            _repository.Add(entity);
            await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

            _logger.LogDebug($"IntegrationEvent: Entity {typeof(TEntity).Name} Created");
        }

        public virtual async Task HandleMessage(EntityUpdated<TEvent> message)
        {
            var entity = _mapper.Map<TEvent, TEntity>(message.NewValue);
            _repository.Update(entity);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

                _logger.LogDebug($"IntegrationEvent: Entity {typeof(TEntity).Name} Updated");
            }
            catch (EntityNotFoundException)
            {
                _repository.Remove(entity);

                EntityCreated<TEvent> created = new EntityCreated<TEvent>(message.NewValue);
                await HandleMessage(created);
            }
        }

        public virtual async Task HandleMessage(EntityDeleted<TEvent> message)
        {
            var entity = _mapper.Map<TEvent, TEntity>(message.Value);
            _repository.Remove(entity);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);

                _logger.LogDebug($"IntegrationEvent: Entity {typeof(TEntity).Name} Removed");
            }
            catch (EntityNotFoundException)
            {
                //Do nothing
                _logger.LogWarning($"IntegrationEvent: Entity {typeof(TEntity).Name} can not be removed beacause is not found");
            }
        }
    }

}
