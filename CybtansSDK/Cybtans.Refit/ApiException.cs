using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Refit
{
    public class ApiException:Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public HttpResponseHeaders Headers { get; }
        public HttpMethod HttpMethod { get; }
        public Uri Uri => RequestMessage.RequestUri;
        public HttpRequestMessage RequestMessage { get; }
        public HttpContentHeaders ContentHeaders { get; private set; }
        public byte[] Content { get; private set; }

        protected ApiException(HttpRequestMessage message, HttpMethod httpMethod, HttpStatusCode statusCode, string reasonPhrase, HttpResponseHeaders headers) :
          base($"Response status code does not indicate success: {(int)statusCode} ({reasonPhrase}).")
        {
            RequestMessage = message;
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Headers = headers;            
        }

        public static async Task<ApiException> Create(HttpRequestMessage message, HttpResponseMessage response)
        {
            var exception = new ApiException(message, message.Method, response.StatusCode, response.ReasonPhrase, response.Headers);

            if (response.Content == null)
            {
                return exception;
            }

            try
            {
                exception.ContentHeaders = response.Content.Headers;
                exception.Content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);               

                response.Content.Dispose();
            }
            catch
            {
                // NB: We're already handling an exception at this point, 
                // so we want to make sure we don't throw another one 
                // that hides the real error.
            }

            return exception;
        }


    }
}
