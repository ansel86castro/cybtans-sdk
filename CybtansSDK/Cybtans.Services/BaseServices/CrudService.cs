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
        : ReadOnlyService<TEntity, TKey, TEntityDto, TGetRequest, TGetAllRequest, TGetAllResponse>
        where TEntity : IEntity<TKey>
        where TEntityDto : IReflectorMetadataProvider, new()
        where TGetRequest : IReflectorMetadataProvider, new()
        where TGetAllRequest : IReflectorMetadataProvider, new()
        where TGetAllResponse : IReflectorMetadataProvider, new()
        where TUpdateRequest : IReflectorMetadataProvider, new()
        where TDeleteRequest : IReflectorMetadataProvider, new()
        where TCreateRequest : IReflectorMetadataProvider, new()
    {       

        public CrudService(
            IRepository<TEntity, TKey> repository,
            IUnitOfWork uow,
            IMapper mapper,
            ILogger logger) : base(repository, uow, mapper, logger)
        {
          
        }

        public virtual async Task<TEntityDto> Create(TCreateRequest request)
        {
            TEntityDto value = typeof(TCreateRequest) == typeof(TEntityDto) ? 
                (TEntityDto)(object)request : 
                GetValue(request);

            var entity = Mapper.Map<TEntity>(value);
            Repository.Add(entity);

            await UoW.SaveChangesAsync();

            Logger?.LogDebug($"Entity {typeof(TEntity)} Created (Id:{entity.Id})");

            return Mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task Delete(TDeleteRequest request)
        {
            TKey id = GetId(request);

            var entity = await Repository.Get(id);
            if (entity == null)
                throw new EntityNotFoundException($"Entity with Id {id} not found");

            Repository.Remove(entity);

            await UoW.SaveChangesAsync();

            Logger?.LogDebug($"Entity {typeof(TEntity)} Deleted (Id:{id})");
        }          

        public virtual async Task<TEntityDto> Update(TUpdateRequest request)
        {
            var id = GetId(request);

            TEntityDto value = typeof(TUpdateRequest) == typeof(TEntityDto) ?
               (TEntityDto)(object)request :
               GetValue(request);

            var entity = await Repository.Get(id);
            if (entity == null)
                throw new EntityNotFoundException($"Entity with Id {id} not found");

            Mapper.Map(value, entity);

            entity.Id = id;

            Repository.Update(entity);
            
            await UoW.SaveChangesAsync();

            Logger?.LogDebug($"Entity {typeof(TEntity)} Updated (Id:{id})");

            return Mapper.Map<TEntity, TEntityDto>(entity);
        }

        protected virtual async Task<TEntityDto> Get(TKey id)
        {
            return (await Mapper.ProjectTo<TEntityDto>(
                Repository.GetAll()
                .Where(x => x.Id.Equals(id)))
                .FirstOrDefaultAsync()) ?? throw new EntityNotFoundException($"Entity with Id {id} not found");
        }

     
        private static TEntityDto GetValue(IReflectorMetadataProvider request)
        {
            var accesor = request.GetAccesor();
            foreach (var code in accesor.GetPropertyCodes())
            {
                if(accesor.GetPropertyType(code) == typeof(TEntityDto))
                {
                    return (TEntityDto)accesor.GetValue(request, code);
                }
            }
            throw new InvalidOperationException($"Property of type {typeof(TEntityDto)} not found");
            
        }
    }
}
