using Cybtans.Messaging;
using Cybtans.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EntityEventPublisher : IEntityEventPublisher
    {
        private readonly IMessageQueue _messageQueue;
        private readonly EntityEventLogContext _context;
        private readonly SubscriptionManager _subscriptionManager;
        private readonly ILogger<EntityEventPublisher> _logger;

        public EntityEventPublisher(EntityEventLogContext context, IMessageQueue messageQueue, SubscriptionManager subscriptionManager, ILogger<EntityEventPublisher>logger)
        {
            _context = context;
            _messageQueue = messageQueue;
            _subscriptionManager = subscriptionManager;
            _logger = logger;
        }

        public async Task Publish(EntityEvent entityEvent)
        {
            var type = entityEvent.GetType();
            EntityEventLog log = new EntityEventLog
            {
                CreateTime = DateTime.Now,
                Data = BinaryConvert.Serialize(entityEvent),
                EntityEventType = type.FullName,
                Id = Guid.NewGuid(),
                State = EventStateEnum.NotPublished
            };
            entityEvent.State = EventStateEnum.NotPublished;

            var binding = _subscriptionManager.GetBindingForType(type);
            if (binding == null)
                throw new QueuePublishException($"Bindindg information not found for {type}");
            log.Exchange = binding.Exchange;
            log.Topic = binding.Topic;

            _context.Events.Add(log);
            try
            {
                await _messageQueue.Publish(entityEvent);
                log.State = EventStateEnum.Published;
                entityEvent.State = EventStateEnum.Published;
            }
            finally
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Unable to save event to the store {type}");
                    throw new EntityEventIntegrationException("Integration events not published", new EntityEvent[] { entityEvent });
                }
            }
        }

        public async Task PublishAll(IEnumerable<EntityEvent> entityEvents)
        {
            var logs = entityEvents.Select(x =>
            {
                var type = entityEvents.GetType();
                EntityEventLog log = new EntityEventLog
                {
                    CreateTime = DateTime.Now,
                    Data = BinaryConvert.Serialize(entityEvents),
                    EntityEventType = type.FullName,
                    Id = Guid.NewGuid(),
                    State = EventStateEnum.NotPublished
                };

                var binding = _subscriptionManager.GetBindingForType(type);
                if (binding == null)
                    throw new QueuePublishException($"Bindindg information not found for {type}");

                log.Exchange = binding.Exchange;
                log.Topic = binding.Topic;
                x.State = EventStateEnum.NotPublished;

                return new { LogEntry = log, Event = x };
            }).ToList();

            _context.Events.AddRange(logs.Select(x => x.LogEntry));

            await Task.WhenAll(logs.Select(async log =>
            {
                try
                {
                    await _messageQueue.Publish(log.Event);
                    log.LogEntry.State = EventStateEnum.Published;
                    log.Event.State = EventStateEnum.Published;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"Unable to save event to the store {log.LogEntry.EntityEventType}");                    
                }
            }));

            await _context.SaveChangesAsync();

            var nonPublished = logs.Where(log => log.Event.State == EventStateEnum.NotPublished).Select(x=>x.Event).ToList();
            if (nonPublished.Count == 0)
            {
                throw new EntityEventIntegrationException("Integration events not published", nonPublished);
            }
        }
    }
}
