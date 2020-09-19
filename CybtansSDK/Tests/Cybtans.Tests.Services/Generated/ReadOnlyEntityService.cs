
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Cybtans.Tests.Domain;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IReadOnlyEntityService))]
    public partial class ReadOnlyEntityService : ReadOnlyService<ReadOnlyEntity, int, ReadOnlyEntityDto, GetReadOnlyEntityRequest, GetAllRequest, GetAllReadOnlyEntityResponse>, IReadOnlyEntityService
    {
        public ReadOnlyEntityService(IRepository<ReadOnlyEntity, int> repository, IUnitOfWork uow, IMapper mapper, ILogger<ReadOnlyEntityService> logger)
            : base(repository, uow, mapper, logger) { }                
    }
}