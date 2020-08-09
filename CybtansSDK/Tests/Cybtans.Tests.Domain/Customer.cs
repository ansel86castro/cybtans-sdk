using Cybtans.Entities;
using System;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Interface)]
    public class Customer: DomainTenantEntity<Guid>
    {
        [EventData]
        public string Name { get; set; }

        [EventData]
        public string FirstLastName { get; set; }

        [EventData]
        public string SecondLastName { get; set; }

        [EventData]
        public Guid? CustomerProfileId { get; set; }

        [EventData]
        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}
