using AutoMapper;
using System.Threading.Tasks;

namespace Cybtans.Entities.EventLog
{
    public class EntityEventsHandler<T> : IEntityEventsHandler<T>
    {
        IRepository<T> _repository;

        public EntityEventsHandler(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task HandleMessage(EntityCreated<T> message)
        {
            _repository.Add(message.Value);
            await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task HandleMessage(EntityUpdated<T> message)
        {
            _repository.Update(message.NewValue);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
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
            }
            catch (EntityNotFoundException)
            {
                //Do nothing
            }
        }
    }

    public class EntityEventsHandler<TEntity, TEvent> : IEntityEventsHandler<TEvent>
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        public EntityEventsHandler(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task HandleMessage(EntityCreated<TEvent> message)
        {
            var entity = _mapper.Map<TEvent, TEntity>(message.Value);
            _repository.Add(entity);
            await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task HandleMessage(EntityUpdated<TEvent> message)
        {
            var entity = _mapper.Map<TEvent, TEntity>(message.NewValue);
            _repository.Update(entity);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
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
            }
            catch (EntityNotFoundException)
            {
                //Do nothing
            }
        }
    }


}
