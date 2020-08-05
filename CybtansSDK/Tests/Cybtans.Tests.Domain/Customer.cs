using Cybtans.Entities;
using System;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Default)]
    public class Customer: DomainTenantEntity<Guid>
    {
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public Guid? CustomerProfileId { get; set; }

        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}
