using Cybtans.Entities;
using System;

namespace Cybtans.Tests.Domain
{
    [GenerateMessage]
    public class CustomerProfile : DomainTenantEntity<Guid>
    {
        public string Name { get; set; }
    }
}
