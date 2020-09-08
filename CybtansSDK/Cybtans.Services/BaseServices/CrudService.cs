using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Cybtans.Entities;
using Cybtans.Serialization;
using Cybtans.Expressions;
using System;

namespace Cybtans.Services
{

    public class CrudService<TEntity, TKey, TEntityDto, TGetRequest, TGetAllRequest, TGetAllResponse, TUpdateRequest, TDeleteRequest>
        : CrudService<TEntity, TKey, TEntityDto, TGetRequest, TGetAllRequest, TGetAllResponse, TUpdateRequest, TEntityDto, TDeleteRequest>
      where TEntity : IEntity<TKey>
      where TEntityDto : IReflectorMetadataProvider, new()
      where TGetRequest : IReflectorMetadataProvider, new()
      where TGetAllRequest : IReflectorMetadataProvider, new()
      where TGetAllResponse : IReflectorMetadataProvider, new()
      where TUpdateRequest : IReflectorMetadataProvider, new()
      where TDeleteRequest : IReflectorMetadataProvider, new()
    {
        public CrudService(IRepository<TEntity, TKey> repository, IUnitOfWork uow, IMapper mapper, ILogger logger) : base(repository, uow, mapper, logger)
        {
        }
    }


    public class CrudService<TEntity, TKey, TEntityDto, TGetRequest, TGetAllRequest, TGetAllResponse, TUpdateRequest, TCreateRequest, TDeleteRequest>
        where TEntity : IEntity<TKey>
        where TEntityDto : IReflectorMetadataProvider, new()
        where TGetRequest : IReflectorMetadataProvider, new()
        where TGetAllRequest : IReflectorMetadataProvider, new()
        where TGetAllResponse : IReflectorMetadataProvider, new()
        where TUpdateRequest : IReflectorMetadataProvider, new()
        where TDeleteRequest : IReflectorMetadataProvider, new()
        where TCreateRequest : IReflectorMetadataProvider, new()
    {
        readonly IRepository<TEntity, TKey> _repository;
        readonly IUnitOfWork _uow;
        readonly ILogger _logger;
        readonly IMapper _mapper;

        public CrudService(
            IRepository<TEntity, TKey> repository,
            IUnitOfWork uow,
            IMapper mapper,
            ILogger logger)
        {
            _repository = repository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        protected ILogger Logger => _logger;
        protected IMapper Mapper => _mapper;
        protected IRepository<TEntity, TKey> Repository => _repository;
        protected IUnitOfWork UoW => _uow;

        public virtual async Task<TEntityDto> Create(TCreateRequest request)
        {            
            var entity = _mapper.Map<TEntity>(request);
            _repository.Add(entity);

            await _uow.SaveChangesAsync();

            _logger.LogDebug($"Entity {typeof(TEntity)} Created (Id:{entity.Id})");

            return _mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task Delete(TDeleteRequest request)
        {
            TKey id = GetId(request);

            var entity = await _repository.Get(id);
            if (entity == null)
                throw new EntityNotFoundException($"Entity with Id {id} not found");

            _repository.Remove(entity);

            await _uow.SaveChangesAsync();

            _logger.LogDebug($"Entity {typeof(TEntity)} Deleted (Id:{id})");
        }      

        public Task<TEntityDto> Get(TGetRequest request)
        {
            return Get(GetId(request));
        }

        public virtual async Task<TGetAllResponse> GetAll(TGetAllRequest request)
        {
            var args = request.Map<GetAllEntitiesRequest>();
            var query = _repository.GetAll();

            if (!string.IsNullOrEmpty(args.Filter))
            {
                query = query.Where(args.Filter);
            }

            if (!string.IsNullOrEmpty(args.Sort))
            {
                query = query.OrderBy(args.Sort);
            }

            var count = await query.LongCountAsync();

            args.Skip ??= 0;

            if(args.Take == null || args.Take <= 0)
            {
                args.Take = 100; 
            }

            var skip = Math.Max(0, (int)args.Skip);
            var take = Math.Max(0, (int)args.Take);

            return new GetAllResponse<TEntityDto>
            {
                Items = await _mapper.ProjectTo<TEntityDto>(query.Skip(skip).Take(take)).ToListAsync(),
                TotalCount = count,
                Page = skip / take,
                TotalPages = count / take + (count % take == 0 ? 0 : 1)
            }.Map<TGetAllResponse>();
        }

        public virtual async Task<TEntityDto> Update(TUpdateRequest request)
        {
            var id = GetId(request);
            var value = GetValue(request);

            var entity = await _repository.Get(id);
            if (entity == null)
                throw new EntityNotFoundException($"Entity with Id {id} not found");

            _mapper.Map(value, entity);

            entity.Id = id;

            _repository.Update(entity);
            
            await _uow.SaveChangesAsync();

            _logger.LogDebug($"Entity {typeof(TEntity)} Updated (Id:{id})");

            return _mapper.Map<TEntity, TEntityDto>(entity);
        }

        protected virtual async Task<TEntityDto> Get(TKey id)
        {
            return (await _mapper.ProjectTo<TEntityDto>(
                _repository.GetAll()
                .Where(x => x.Id.Equals(id)))
                .FirstOrDefaultAsync()) ?? throw new EntityNotFoundException($"Entity with Id {id} not found");
        }


        private static TKey GetId(IReflectorMetadataProvider request)
        {
            var code = request.GetAccesor().GetPropertyCode(nameof(IEntity<TKey>.Id));
            var id = (TKey)request.GetAccesor().GetValue(request, code);
            return id;
        }

        private static TEntityDto GetValue(IReflectorMetadataProvider request)
        {
            var code = request.GetAccesor().GetPropertyCode("Value");
            var value = (TEntityDto)request.GetAccesor().GetValue(request, code);
            return value;
        }
    }
}
