using Cybtans.Entities;

namespace Cybtans.Tests.Domain
{
    [GenerateMessage(Service = ServiceType.Default, Security = SecurityType.Role, AllowedRead = "admin", AllowedWrite = "admin")]
    public class OrderState : Entity<int>
    {
        public string Name { get; set; }
    }
}
