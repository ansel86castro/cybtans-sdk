using Cybtans.Entities;
using Cybtans.Messaging;
using Microsoft.Extensions.Logging;
using Ordering.Models;
using System.Threading.Tasks;

namespace Ordering.RestApi
{
    public class OrderCreatedProcessor : IMessageHandler<EntityCreated<Order>>
    {
        IRepository<Order> _repository;
        ILogger<OrderCreatedProcessor> _logger;
        public OrderCreatedProcessor(IRepository<Order> repository, ILogger<OrderCreatedProcessor> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task HandleMessage(EntityCreated<Order> message)
        {
            _logger.LogInformation("Order Created");
            await Task.Delay(100);
        }
    }
}