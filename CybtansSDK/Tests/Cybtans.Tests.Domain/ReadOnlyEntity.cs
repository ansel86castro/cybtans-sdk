using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Tests.Domain
{
    [GenerateMessage(Service = ServiceType.ReadOnly, Security = SecurityType.Role, AllowedRead = "admin")]
    public class ReadOnlyEntity:AuditableEntity<int>
    {
        public string Name { get; set; }
    }
}
