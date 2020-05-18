using System;

#nullable enable

namespace Cybtans.Proto
{
    public class TypeNotFoundExcetion : Exception
    {
        public TypeNotFoundExcetion(string? message =null) : base(message) { }

        public TypeNotFoundExcetion()
        {
        }

        public TypeNotFoundExcetion(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

#nullable disable