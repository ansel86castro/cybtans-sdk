using Cybtans.Entities;

namespace Cybtans.Test.Domain
{
    [GenerateMessage]
    public class OrderState: Entity<int>
    {
        public string Name { get; set; }
    }
}
