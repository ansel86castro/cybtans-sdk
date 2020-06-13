using System;
using System.ComponentModel.DataAnnotations;
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

            Console.WriteLine("Hello World!");
        }
    }
}
