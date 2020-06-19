using Cybtans.Messaging;
using Cybtans.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities.EventLog
{
  
    public class EntityEventPublisher : IEntityEventPublisher
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IRepository<EntityEventLog, Guid>? _context;
        private readonly ILogger<EntityEventPublisher>? _logger;

        public EntityEventPublisher(
            IMessageQueue messageQueue,
            IRepository<EntityEventLog, Guid>? context = null,
            ILogger<EntityEventPublisher>? logger = null)
        {
            _context = context;
            _messageQueue = messageQueue;
            _logger = logger;
        }

        private async Task<EntityEventLog> PublishInternal(EntityEvent entityEvent)
        {

            var type = entityEvent.GetType();
            EntityEventLog log = new EntityEventLog
            {
                CreateTime = DateTime.Now,
                Data = BinaryConvert.Serialize(entityEvent),
                EntityEventType = type.FullName,
                State = EventStateEnum.NotPublished
            };
            entityEvent.State = EventStateEnum.NotPublished;

            var binding = _messageQueue.GetBinding(type);
            if (binding == null)
                throw new QueuePublishException($"Bindindg information not found for {type}");

            log.Exchange = binding.Exchange;
            log.Topic = binding.Topic;

            try
            {
                entityEvent.State = EventStateEnum.Published;
                log.State = EventStateEnum.Published;
                await _messageQueue.Publish(entityEvent).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                entityEvent.State = EventStateEnum.NotPublished;
                log.State = EventStateEnum.NotPublished;
                log.ErrorMessage = ex.Message;

                _logger?.LogCritical(ex, $"Unable to publish event of type {type}");
            }

            return log;
        }

        public async Task Publish(EntityEvent entityEvent)
        {
            var log = await PublishInternal(entityEvent).ConfigureAwait(false);
            if (_context != null)
            {
                if (entityEvent.Id == Guid.Empty)
                {
                    entityEvent.Id = Guid.NewGuid();
                    _context.Add(log);
                }
                else
                {
                    log.Id = entityEvent.Id;
                    _context.Update(log);
                }

                await _context.UnitOfWork.SaveChangesAsync();
                entityEvent.Id = log.Id;
            }
        }

        public async Task PublishAll(IEnumerable<EntityEvent> entityEvents)
        {
            var logs = new List<(EntityEventLog, EntityEvent)>();
            foreach (var item in entityEvents)
            {
                logs.Add((await PublishInternal(item), item));
            }

            if (_context != null)
            {

                foreach (var (log, ev) in logs)
                {
                    if (ev.Id == Guid.Empty)
                    {
                        ev.Id = Guid.NewGuid();
                        _context.Add(log);
                    }
                    else
                    {
                        log.Id = ev.Id;
                        _context.Update(log);
                    }
                }

                await _context.UnitOfWork.SaveChangesAsync();
                var nonPublished = entityEvents.Where(e => e.State == EventStateEnum.NotPublished).ToList();
                if (nonPublished.Count > 0)
                {
                    throw new EntityEventIntegrationException("Integration events not published", nonPublished);
                }
            }
        }
    }
}
