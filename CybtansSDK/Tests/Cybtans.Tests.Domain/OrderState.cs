using Cybtans.Entities;

namespace Cybtans.Test.Domain
{
    [GenerateMessage(Service = ServiceType.Default)]
    public class OrderState: Entity<int>
    {
        public string Name { get; set; }
    }
}
