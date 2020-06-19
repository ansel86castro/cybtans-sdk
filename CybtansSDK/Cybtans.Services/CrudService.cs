using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Cybtans.Entities;
using Cybtans.Serialization;
using Cybtans.Expressions;

namespace Cybtans.Services
{
    internal static class IReflectorExtensor
    {
        public static TOut Map<TOut>(this IReflectorMetadataProvider obj) where TOut : IReflectorMetadataProvider, new()
        {
            var accesor = obj.GetAccesor();
            TOut clone = new TOut();
            var cloneAccesor = clone.GetAccesor();

            foreach (var code in accesor.GetPropertyCodes())
            {
                cloneAccesor.SetValue(clone, code, accesor.GetValue(obj, code));
            }

            return clone;
        }
    }

    internal class GetAllResponse<T> : IReflectorMetadataProvider
    {
        private static readonly GetAllResponseAccesor __accesor = new GetAllResponseAccesor();

        public List<T> Items { get; set; }

        public long Page { get; set; }

        public long TotalPages { get; set; }

        public long TotalCount { get; set; }

        public IReflectorMetadata GetAccesor()
        {
            return __accesor;
        }

        public sealed class GetAllResponseAccesor : IReflectorMetadata
        {
            public const int Items = 1;
            public const int Page = 2;
            public const int TotalPages = 3;
            public const int TotalCount = 4;
            private readonly int[] _props = new[]
            {
                Items,Page,TotalPages,TotalCount
            };

            public int[] GetPropertyCodes() => _props;

            public string GetPropertyName(int propertyCode)
            {
                return propertyCode switch
                {
                    Items => "Items",
                    Page => "Page",
                    TotalPages => "TotalPages",
                    TotalCount => "TotalCount",
                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public int GetPropertyCode(string propertyName)
            {
                return propertyName switch
                {
                    "Items" => Items,
                    "Page" => Page,
                    "TotalPages" => TotalPages,
                    "TotalCount" => TotalCount,
                    _ => -1,
                };
            }

            public Type GetPropertyType(int propertyCode)
            {
                return propertyCode switch
                {
                    Items => typeof(List<T>),
                    Page => typeof(long),
                    TotalPages => typeof(long),
                    TotalCount => typeof(long),
                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public object GetValue(object target, int propertyCode)
            {
                GetAllResponse<T> obj = (GetAllResponse<T>)target;
                return propertyCode switch
                {
                    Items => obj.Items,
                    Page => obj.Page,
                    TotalPages => obj.TotalPages,
                    TotalCount => obj.TotalCount,
                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public void SetValue(object target, int propertyCode, object value)
            {
                GetAllResponse<T> obj = (GetAllResponse<T>)target;
                switch (propertyCode)
                {
                    case Items: obj.Items = (List<T>)value; break;
                    case Page: obj.Page = (long)value; break;
                    case TotalPages: obj.TotalPages = (long)value; break;
                    case TotalCount: obj.TotalCount = (long)value; break;
                    default: throw new InvalidOperationException("property code not supported");
                }
            }

        }
    }

    internal class UpdateRequest<T, TKey> : IReflectorMetadataProvider
    {
        private static readonly UpdateBuildingRequestAccesor __accesor = new UpdateBuildingRequestAccesor();

        public TKey Id { get; set; }

        public T Value { get; set; }

        public IReflectorMetadata GetAccesor()
        {
            return __accesor;
        }

        public sealed class UpdateBuildingRequestAccesor : IReflectorMetadata
        {
            public const int Id = 1;
            public const int Value = 2;
            private readonly int[] _props = new[]
            {
            Id,Value
        };

            public int[] GetPropertyCodes() => _props;

            public string GetPropertyName(int propertyCode)
            {
                return propertyCode switch
                {
                    Id => "Id",
                    Value => "Value",

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public int GetPropertyCode(string propertyName)
            {
                return propertyName switch
                {
                    "Id" => Id,
                    "Value" => Value,

                    _ => -1,
                };
            }

            public Type GetPropertyType(int propertyCode)
            {
                return propertyCode switch
                {
                    Id => typeof(TKey),
                    Value => typeof(T),

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public object GetValue(object target, int propertyCode)
            {
                UpdateRequest<T, TKey> obj = (UpdateRequest<T, TKey>)target;
                return propertyCode switch
                {
                    Id => obj.Id,
                    Value => obj.Value,

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public void SetValue(object target, int propertyCode, object value)
            {
                UpdateRequest<T, TKey> obj = (UpdateRequest<T, TKey>)target;
                switch (propertyCode)
                {
                    case Id: obj.Id = (TKey)value; break;
                    case Value: obj.Value = (T)value; break;

                    default: throw new InvalidOperationException("property code not supported");
                }
            }

        }
    }

    internal partial class GetAllEntitiesRequest : IReflectorMetadataProvider
    {
        private static readonly GetAllRequestAccesor __accesor = new GetAllRequestAccesor();

        public string Filter { get; set; }

        public string Sort { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public IReflectorMetadata GetAccesor()
        {
            return __accesor;
        }

        public sealed class GetAllRequestAccesor : IReflectorMetadata
        {
            public const int Filter = 1;
            public const int Sort = 2;
            public const int Skip = 3;
            public const int Take = 4;
            private readonly int[] _props = new[]
            {
            Filter,Sort,Skip,Take
        };

            public int[] GetPropertyCodes() => _props;

            public string GetPropertyName(int propertyCode)
            {
                return propertyCode switch
                {
                    Filter => "Filter",
                    Sort => "Sort",
                    Skip => "Skip",
                    Take => "Take",

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public int GetPropertyCode(string propertyName)
            {
                return propertyName switch
                {
                    "Filter" => Filter,
                    "Sort" => Sort,
                    "Skip" => Skip,
                    "Take" => Take,

                    _ => -1,
                };
            }

            public Type GetPropertyType(int propertyCode)
            {
                return propertyCode switch
                {
                    Filter => typeof(string),
                    Sort => typeof(string),
                    Skip => typeof(int),
                    Take => typeof(int),

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public object GetValue(object target, int propertyCode)
            {
                GetAllEntitiesRequest obj = (GetAllEntitiesRequest)target;
                return propertyCode switch
                {
                    Filter => obj.Filter,
                    Sort => obj.Sort,
                    Skip => obj.Skip,
                    Take => obj.Take,

                    _ => throw new InvalidOperationException("property code not supported"),
                };
            }

            public void SetValue(object target, int propertyCode, object value)
            {
                GetAllEntitiesRequest obj = (GetAllEntitiesRequest)target;
                switch (propertyCode)
                {
                    case Filter: obj.Filter = (string)value; break;
                    case Sort: obj.Sort = (string)value; break;
                    case Skip: obj.Skip = (int)value; break;
                    case Take: obj.Take = (int)value; break;

                    default: throw new InvalidOperationException("property code not supported");
                }
            }

        }
    }

    public class CrudService<TEntity, TKey, TEntityDto, TGetRequest, TGetAllRequest, TGetAllResponse, TUpdateRequest, TDeleteRequest>
        where TEntity : IEntity<TKey>
        where TGetRequest : IReflectorMetadataProvider, new()
        where TGetAllRequest : IReflectorMetadataProvider, new()
        where TGetAllResponse : IReflectorMetadataProvider, new()
        where TUpdateRequest : IReflectorMetadataProvider, new()
        where TDeleteRequest : IReflectorMetadataProvider, new()
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

        public virtual async Task<TEntityDto> Create(TEntityDto request)
        {
            var entity = _mapper.Map<TEntityDto, TEntity>(request);
            _repository.Add(entity);

            await _uow.SaveChangesAsync();

            _logger.LogDebug($"Entity {typeof(TEntity)} saved id:{entity.Id}");

            return _mapper.Map<TEntity, TEntityDto>(entity);
        }

        public virtual async Task Delete(TDeleteRequest request)
        {
            TKey id = GetId(request);

            var entity = await _repository.Get(id);
            _repository.Remove(entity);

            await _uow.SaveChangesAsync();
        }

        protected virtual async Task<TEntityDto> Get(TKey id)
        {
            return await _mapper.ProjectTo<TEntityDto>(
                _repository.GetAll()
                .Where(x => x.Id.Equals(id)))
                .FirstOrDefaultAsync();
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

            return new GetAllResponse<TEntityDto>
            {
                Items = await _mapper.ProjectTo<TEntityDto>(query.Skip(args.Skip).Take(args.Take)).ToListAsync(),
                TotalCount = count,
                Page = args.Skip / args.Take,
                TotalPages = count / args.Take + (count % args.Take == 0 ? 0 : 1)
            }.Map<TGetAllResponse>();
        }

        public virtual async Task<TEntityDto> Update(TUpdateRequest request)
        {
            var id = GetId(request);
            var value = GetValue(request);

            var entity = await _repository.Get(id);
            _mapper.Map(value, entity);

            entity.Id = id;

            _repository.Update(entity);

            await _uow.SaveChangesAsync();

            return _mapper.Map<TEntity, TEntityDto>(entity);
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
