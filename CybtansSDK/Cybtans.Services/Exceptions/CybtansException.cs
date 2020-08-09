using System;
using System.Net;
using System.Runtime.Serialization;

#nullable enable

namespace Cybtans.Services
{
    public class CybtansException : Exception
    {
        public CybtansException()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public CybtansException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public CybtansException(string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public CybtansException(string message, Exception innerException, HttpStatusCode statusCode, int? errorCode) : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        public CybtansException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            ErrorCode = (int)statusCode;
        }

        public CybtansException(HttpStatusCode statusCode, string message):base(message)
        {
            StatusCode = statusCode;
            ErrorCode = (int)statusCode;
        }

        protected CybtansException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public HttpStatusCode StatusCode { get; }

        public int? ErrorCode { get; }
    }
}
