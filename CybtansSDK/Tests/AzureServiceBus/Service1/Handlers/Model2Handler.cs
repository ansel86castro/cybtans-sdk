using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using Service1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service1.Handlers
{
    public class Model2Handler : IEntityEventsHandler<Model2>
    {
        private readonly ILogger<Model2Handler> _logger ;

        public Model2Handler(ILogger<Model2Handler> logger)
        {
            _logger  = logger;
        }

        public Task HandleMessage(EntityCreated<Model2> message)
        {
            _logger.LogInformation("Entity Created {Id} {Name}", message.Value.Id, message.Value.Name);
            return Task.CompletedTask;
        }

        public Task HandleMessage(EntityUpdated<Model2> message)
        {
            _logger.LogInformation("Entity Updated Old {Id} {Name}", message.OldValue.Id, message.OldValue.Name);
            _logger.LogInformation("Entity Updated New {Id} {Name}", message.NewValue.Id, message.NewValue.Name);
            return Task.CompletedTask;
        }

        public Task HandleMessage(EntityDeleted<Model2> message)
        {
            _logger.LogInformation("Entity Deleted {Id} {Name}", message.Value.Id, message.Value.Name);
            return Task.CompletedTask;
        }
    }
}
