using AutoMapper;
using Cybtans.Entities;
using Cybtans.Serialization;
using Microsoft.Extensions.Logging;
using Cybtans.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Cybtans.Services.Extensions;
using Cybtans.Entities.Extensions;

namespace Cybtans.Services
{
    public class ReadOnlyService<TEntity, TKey, TEntityDto, TGetRequest, TGetAllRequest, TGetAllResponse>
        where TEntity : IEntity<TKey>
        where TEntityDto : IReflectorMetadataProvider, new()
        where TGetRequest : IReflectorMetadataProvider, new()
        where TGetAllRequest : IReflectorMetadataProvider, new()
        where TGetAllResponse : IReflectorMetadataProvider, new()      
    {
        readonly IRepository<TEntity, TKey> _repository;
        readonly IUnitOfWork _uow;
        readonly ILogger _logger;
        readonly IMapper _mapper;

        public ReadOnlyService(
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
      
        public Task<TEntityDto> Get(TGetRequest request)
        {
            return Get(GetId(request));
        }

        public virtual async Task<TGetAllResponse> GetAll(TGetAllRequest request)
        {
            var args = request.Map<GetAllEntitiesRequest>();

            var page = await _repository.GetAll(consistency: ReadConsistency.Weak).PageBy(args.Filter, args.Sort, args.Skip, args.Take);
            return new GetAllResponse<TEntityDto>
            {
                Items = await _mapper.ProjectTo<TEntityDto>(page.Query).ToListAsync(),
                TotalCount = page.TotalCount,
                Page = page.Page,
                TotalPages = page.TotalPages
            }.Map<TGetAllResponse>();
        }


        protected virtual async Task<TEntityDto> Get(TKey id)
        {
            return (await _mapper.ProjectTo<TEntityDto>(
                _repository.GetAll(consistency: ReadConsistency.Weak)
                .Where(x => x.Id.Equals(id)))
                .FirstOrDefaultAsync()) ?? throw new EntityNotFoundException($"Entity with Id {id} not found");
        }


        protected TKey GetId(IReflectorMetadataProvider request)
        {
            var code = request.GetAccesor().GetPropertyCode(nameof(IEntity<TKey>.Id));
            var id = (TKey)request.GetAccesor().GetValue(request, code);
            return id;
        }       

    }
}
