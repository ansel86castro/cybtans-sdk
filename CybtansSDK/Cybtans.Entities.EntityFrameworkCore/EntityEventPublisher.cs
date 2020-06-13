using Cybtans.Messaging;
using Cybtans.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Entities.EntityFrameworkCore
{
    public class EntityEventPublisher : IEntityEventPublisher
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IRepository<EntityEventLog, Guid>? _context;        
        private readonly ILogger<EntityEventPublisher>? _logger;

        public EntityEventPublisher(IMessageQueue messageQueue, IRepository<EntityEventLog, Guid>? context = null, ILogger<EntityEventPublisher>? logger = null)
        {
            _context = context;
            _messageQueue = messageQueue;            
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

            var binding = _messageQueue.GetBindingForType(type);
            if (binding == null)
                throw new QueuePublishException($"Bindindg information not found for {type}");
            log.Exchange = binding.Exchange;
            log.Topic = binding.Topic;

            if (_context == null)
            {
                try
                {
                    await _messageQueue.Publish(entityEvent);
                    entityEvent.State = EventStateEnum.Published;
                }
                catch(Exception ex)
                {
                    _logger?.LogCritical(ex, $"Unable to publish {type}");
                    throw ex;
                }
            }

            else
            {
                _context.Add(log);
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
                        await _context.UnitOfWork.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogCritical(ex, $"Unable to save event to the store {type}");
                        throw new EntityEventIntegrationException("Integration events not published", new EntityEvent[] { entityEvent });
                    }
                }
            }
        }

        public async Task PublishAll(IEnumerable<EntityEvent> entityEvents)
        {
            var logs = entityEvents.Select(x =>
            {
                var type = x.GetType();
                EntityEventLog log = new EntityEventLog
                {
                    CreateTime = DateTime.Now,
                    Data = BinaryConvert.Serialize(x),
                    EntityEventType = type.FullName,
                    Id = Guid.NewGuid(),
                    State = EventStateEnum.NotPublished
                };

                var binding = _messageQueue.GetBindingForType(type);
                if (binding == null)
                    throw new QueuePublishException($"Bindindg information not found for {type}");

                log.Exchange = binding.Exchange;
                log.Topic = binding.Topic;
                x.State = EventStateEnum.NotPublished;

                return new { LogEntry = log, Event = x };
            }).ToList();

            if (_context == null)
            {
                await Task.WhenAll(logs.Select(async log =>
                {
                    try
                    {
                        await _messageQueue.Publish(log.Event);
                        log.Event.State = EventStateEnum.Published;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogCritical(ex, $"Unable to save event to the store {log.LogEntry.EntityEventType}");
                        throw ex;
                    }
                }));
            }
            else
            {
                _context.AddRange(logs.Select(x => x.LogEntry));

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
                        _logger?.LogCritical(ex, $"Unable to save event to the store {log.LogEntry.EntityEventType}");
                    }
                }));

                await _context.UnitOfWork.SaveChangesAsync();

                var nonPublished = logs.Where(log => log.Event.State == EventStateEnum.NotPublished).Select(x => x.Event).ToList();
                if (nonPublished.Count > 0)
                {
                    throw new EntityEventIntegrationException("Integration events not published", nonPublished);
                }
            }
        }
    }
}
