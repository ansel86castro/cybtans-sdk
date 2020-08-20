using Cybtans.Serialization;
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

        public string RemoteExceptionMessage { get; private set; }

        public string RemoteExceptionStackTrace { get; private set; }

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

                var errorInfo = ToErrorInfo(exception);
                if(errorInfo != null)
                {
                    exception.RemoteExceptionMessage = errorInfo.ErrorMessage;
                    exception.RemoteExceptionStackTrace = errorInfo.StackTrace;
                }
            }
            catch
            {
                // NB: We're already handling an exception at this point, 
                // so we want to make sure we don't throw another one 
                // that hides the real error.
            }

            return exception;
        }

        private static ErrorInfo? ToErrorInfo(ApiException apiException)
        {
            ErrorInfo? result = null;

            if (apiException.Content != null)
            {
                if (apiException.ContentHeaders?.ContentType?.MediaType == BinarySerializer.MEDIA_TYPE)
                {
                    if (apiException.ContentHeaders.ContentType.CharSet == BinarySerializer.DefaultEncoding.WebName)
                    {
                        result = BinaryConvert.Deserialize<ErrorInfo>(apiException.Content);
                    }
                    else
                    {
                        var encoding = Encoding.GetEncoding(apiException.ContentHeaders.ContentType.CharSet);
                        var serializer = new BinarySerializer(encoding);
                        result = serializer.Deserialize<ErrorInfo>(apiException.Content);
                    }
                }
                else if (IsJson(apiException.ContentHeaders?.ContentType?.MediaType) && apiException.StatusCode == HttpStatusCode.BadRequest)
                {
                    var encoding = apiException.ContentHeaders?.ContentType?.CharSet != null ?
                        Encoding.GetEncoding(apiException.ContentHeaders.ContentType.CharSet) :
                        Encoding.UTF8;

                    var json = encoding.GetString(apiException.Content);
                    var apiValidation = System.Text.Json.JsonSerializer.Deserialize<FluentValidationResult>(json);

                    result = new ErrorInfo { ErrorMessage = apiValidation.title, ErrorCode = (int)apiException.StatusCode };                    
                }
            }
           

            return result;
        }

        private static bool IsJson(string? mediaType)
        {
            return mediaType == "application/problem+json" || mediaType == "application/json";
        }

        private class FluentValidationResult
        {
            public int status { get; set; }
            public string? title { get; set; }
            public Dictionary<string, List<string>>? errors { get; set; }

        }

        private class ErrorInfo
        {
            public string ErrorMessage { get; set; }

            public string StackTrace { get; set; }

            public int? ErrorCode { get; set; }
        }          

    }
}
