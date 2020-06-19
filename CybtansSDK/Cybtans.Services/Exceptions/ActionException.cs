using System;
using System.Net;
using System.Runtime.Serialization;

namespace Cybtans.Services
{
    public class ActionException : Exception
    {
        public ActionException()
        {
        }

        public ActionException(string message) : base(message)
        {
        }

        public ActionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ActionException(HttpStatusCode statusCode, object response, string message = null) : base(message)
        {
            StatusCode = statusCode;
            Response = response;
        }

        protected ActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public HttpStatusCode? StatusCode { get; set; }

        public object Response { get; set; }
    }
}
