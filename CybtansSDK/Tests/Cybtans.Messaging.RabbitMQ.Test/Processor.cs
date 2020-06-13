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
    public class Processor
    {
        private RabbitMessageQueue _messageQueue;
        public static AutoResetEvent Wait = new AutoResetEvent(false);

        public static List<EntityEvent> Events = new List<EntityEvent>();

        public Processor()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _messageQueue = new RabbitMessageQueue(factory, logger: loggerFactory.CreateLogger<RabbitMessageQueue>());
            _messageQueue.Subscribe<InvoiceCreated, InvoiceCreateHandler>();          
        }

        public class InvoiceCreateHandler : IMessageHandler<InvoiceCreated>
        {             
            public Task HandleMessage(InvoiceCreated message)
            {
                Console.WriteLine("Invoice Created");

                lock (Events)
                {
                    Events.Add(message);
                }

                Wait.Set();
                return Task.CompletedTask;
            }
        }
    }
}
