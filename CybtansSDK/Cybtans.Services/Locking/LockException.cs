using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Cybtans.Services.Locking
{
    public class LockException : InvalidOperationException
    {
        public LockException()
        {
        }

        public LockException(string message) : base(message)
        {
        }

        public LockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
