using Cybtans.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Messaging.RabbitMQ.Test
{
    [ExchangeRoute("Invoice")]
    public class InvoiceCreated: EntityEvent
    {
        public Invoice Value { get; private set; }

        public InvoiceCreated()
        {

        }

        public InvoiceCreated(Invoice value)
        {
            Value = value;
        }
    }

    [ExchangeRoute("Invoice")]
    public class InvoiceDeleted : EntityEvent
    {
        public Invoice Value { get; private set; }

        public InvoiceDeleted() { }

        public InvoiceDeleted(Invoice value)
        {
            Value = value;
        }
    }

    [ExchangeRoute("Invoice")]
    public class InvoiceUpdated : EntityEvent
    {
        public InvoiceUpdated() { }

        public InvoiceUpdated(Invoice oldValue, Invoice newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public Invoice OldValue { get; set; }

        public Invoice NewValue { get; set; }
    }
}
