using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Service2.Models;
using System.Threading.Tasks;

namespace Service2.Handlers
{
    public class Model1Handler : IEntityEventsHandler<Model1>
    {
        private readonly ILogger<Model1Handler> _logger;

        public Model1Handler(ILogger<Model1Handler> logger)
        {
            _logger = logger;
        }

        public Task HandleMessage(EntityCreated<Model1> message)
        {
            _logger.LogInformation("Entity Created {Id} {Name}", message.Value.Id, message.Value.Name);
            return Task.CompletedTask;
        }

        public Task HandleMessage(EntityUpdated<Model1> message)
        {
            _logger.LogInformation("Entity Updated Old {Id} {Name}", message.OldValue.Id, message.OldValue.Name);
            _logger.LogInformation("Entity Updated New {Id} {Name}", message.NewValue.Id, message.NewValue.Name);
            return Task.CompletedTask;
        }

        public Task HandleMessage(EntityDeleted<Model1> message)
        {
            _logger.LogInformation("Entity Deleted {Id} {Name}", message.Value.Id, message.Value.Name);
            return Task.CompletedTask;
        }
    }
}
