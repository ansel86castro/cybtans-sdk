using Cybtans.Entities;
using System;

namespace Cybtans.Tests.Domain
{
    [GenerateMessage(Service = ServiceType.Default, Security = SecurityType.Authorized)]
    public class CustomerEvent : Entity<Guid>
    {
        public string FullName { get; set; }

        public Guid? CustomerProfileId { get; set; }

    }
}
