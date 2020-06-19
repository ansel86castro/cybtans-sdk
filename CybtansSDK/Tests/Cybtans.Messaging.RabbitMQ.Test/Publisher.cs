using Cybtans.Entities;
using Cybtans.Entities.EntityFrameworkCore;
using Cybtans.Entities.EntiyFrameworkCore;
using Cybtans.Entities.EventLog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cybtans.Messaging.RabbitMQ.Test
{
    public class Publisher
    {
        IMessageQueue _messageQueue;
        IEntityEventPublisher _eventPublisher;
        TestContext _context;
        EfUnitOfWork _uow;

        public Publisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            
            var loggerFactory =  LoggerFactory.Create(builder=>
            {
                builder.AddConsole();
            });            
            
            _messageQueue = new RabbitMessageQueue(factory, new RabbitMessageQueueOptions
            {
                Exchange = new ExchangeConfig
                {
                    Name = "Invoice"
                }
            }, logger:loggerFactory.CreateLogger<RabbitMessageQueue>());
            _context = TestContext.Create();
            _context.Database.EnsureCreated();

            var eventLogUoW = new EfUnitOfWork(_context);             
            var repository = new EfRepository<EntityEventLog, Guid>(eventLogUoW);
            _eventPublisher = new EntityEventPublisher(_messageQueue, repository, loggerFactory.CreateLogger<EntityEventPublisher>());

            _uow = new EfUnitOfWork(_context, _eventPublisher);
        }

        public async Task Publish()
        {
            var invoice = CreateInvoice();          
            _context.Add(invoice);

            await _uow.SaveChangesAsync();

            invoice.Code = Guid.NewGuid().ToString();

            _context.Update(invoice);

            await _uow.SaveChangesAsync();
        }

        public Invoice CreateInvoice()
        {
            return new Invoice
            {                
                Code = "Invoice",
                Items = new List<InvoiceItems>
                {
                    new InvoiceItems
                    {
                        Price = 5,
                            Product = "Product 1"
                    },
                    new InvoiceItems
                    {
                        Price= 10,
                            Product = "Product 2"
                    }
                }
            };
        }
    }
}
