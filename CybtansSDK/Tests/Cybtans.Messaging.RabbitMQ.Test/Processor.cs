using Cybtans.Entities;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cybtans.Messaging.RabbitMQ.Test
{
    public class Processor : IMessageHandler<EntityUpdated<Invoice>>
    {
        private RabbitMessageQueue _messageQueue;
        public static AutoResetEvent Wait = new AutoResetEvent(false);

        public static List<EntityEvent> Events = new List<EntityEvent>();

        public Processor(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _messageQueue = new RabbitMessageQueue(factory, new RabbitMessageQueueOptions
            {
                Exchange = new ExchangeConfig
                {
                    Name = "Invoice"
                },
                Queue = new QueueConfig
                {
                    Name = queueName
                }
            },
            logger: loggerFactory.CreateLogger<RabbitMessageQueue>());
            _messageQueue.Subscribe<EntityCreated<Invoice>, InvoiceCreateHandler>();
            _messageQueue.Subscribe(this);
        }

        public Task HandleMessage(EntityUpdated<Invoice> message)
        {
            Console.WriteLine("Invoice Updated");
            return Task.CompletedTask;
        }

        public class InvoiceCreateHandler : IMessageHandler<EntityCreated<Invoice>>
        {             
            public Task HandleMessage(EntityCreated<Invoice> message)
            {
                Console.WriteLine("Invoice Created");

                lock (Events)
                {
                    Events.Add(message);
                }
                
                return Task.CompletedTask;
            }
        }

     
    }
}
