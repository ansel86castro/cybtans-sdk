using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Messaging
{
    public class MessageQueueException : Exception
    {
        public MessageQueueException() { }

        public MessageQueueException(string message) : base(message)
        {
        }

        public MessageQueueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class QueueConnectionException : MessageQueueException
    {
        public QueueConnectionException(string message) : base(message)
        {
        }

        public QueueConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class QueuePublishException: MessageQueueException
    {
        public QueuePublishException(string message, object data = null) : base(message)
        {
            PublishData = data;
        }

        public object PublishData { get; }

        public QueuePublishException()
        {
        }

        public QueuePublishException(string message) : base(message)
        {
        }

        public QueuePublishException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class QueueSubscribeException : MessageQueueException
    {
        public QueueSubscribeException(string message) : base(message) { }

        public QueueSubscribeException()
        {
        }

        public QueueSubscribeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
