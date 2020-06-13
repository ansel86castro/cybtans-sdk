using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Cybtans.Messaging.RabbitMQ.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = new Publisher();
            var processor = new Processor();

            publisher.AddInvoices().GetAwaiter().GetResult();

            Processor.Wait.WaitOne(5000);

            var context = TestContext.Create();

            var logs = context.Events.ToList();

            Console.WriteLine($"Events saved :{logs.Count}");
            Console.WriteLine($"Events published :{logs.Count(x => x.State == Entities.EventStateEnum.Published)}");
            Console.WriteLine($"Events received: {Processor.Events.Count}");
        }
    }
}
