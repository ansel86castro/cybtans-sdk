using Cybtans.Entities;
using System;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Interface)]
    public class Customer: DomainTenantEntity<Guid>
    {
        [EventMember]
        public string Name { get; set; }

        [EventMember]
        public string FirstLastName { get; set; }

        [EventMember]
        public string SecondLastName { get; set; }

        [EventMember]
        public Guid? CustomerProfileId { get; set; }

        [EventMember]
        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}
