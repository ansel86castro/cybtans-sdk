using Cybtans.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Refit
{
    public class ApiException:Exception
    {
        public HttpStatusCode StatusCode { get; }        
        public HttpResponseHeaders Headers { get; }
        public HttpMethod HttpMethod { get; }
        public Uri Uri => RequestMessage.RequestUri;
        public HttpRequestMessage RequestMessage { get; }
        public HttpContentHeaders? ContentHeaders { get; private set; }
        public byte[]? Content { get; private set; }
        public ApiErrorInfo? Errors { get; private set; }

        protected ApiException(HttpRequestMessage request, HttpMethod httpMethod, HttpStatusCode statusCode, HttpResponseHeaders headers, string message) :
          base(message)
        {
            RequestMessage = request;
            HttpMethod = httpMethod;
            StatusCode = statusCode;            
            Headers = headers;            
        }        

        public static async Task<ApiException> Create(HttpRequestMessage message, HttpResponseMessage response)
        {
            byte[]? content = null;
            ApiErrorInfo? info = null;

            if(response.Content != null)
            {
                try
                {
                    content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    info = ToErrorInfo(response.StatusCode, response.Content.Headers, content);

                    response.Content.Dispose();
                }
                catch
                {
                    // NB: We're already handling an exception at this point, 
                    // so we want to make sure we don't throw another one 
                    // that hides the real error.
                }
            }

            var msg = $"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).";
            if(info != null)
            {
                msg += "\r\n" + info.ToString();
            }

            var exception = new ApiException(message, message.Method, response.StatusCode, response.Headers, msg);
            if(response.Content != null)
            {
                exception.ContentHeaders = response.Content.Headers;
                exception.Content = content;
                exception.Errors = info;
            }                       

            return exception;
        }


        private static ApiErrorInfo? ToErrorInfo(HttpStatusCode statusCode, HttpContentHeaders contentHeaders, byte[]content )
        {
            ApiErrorInfo? result = null;

            var contentType = contentHeaders.ContentType;

            if (content != null)
            {
                if (contentType?.MediaType == BinarySerializer.MEDIA_TYPE)
                {
                    if (contentHeaders.ContentType.CharSet == BinarySerializer.DefaultEncoding.WebName)
                    {
                        result = BinaryConvert.Deserialize<ApiErrorInfo>(content);
                    }
                    else
                    {
                        var encoding = Encoding.GetEncoding(contentHeaders.ContentType.CharSet);
                        var serializer = new BinarySerializer(encoding);
                        result = serializer.Deserialize<ApiErrorInfo>(content);
                    }
                }
                else if (IsJson(contentType?.MediaType))
                {
                    var encoding = contentType?.CharSet != null ?
                        Encoding.GetEncoding(contentHeaders.ContentType.CharSet) :
                        Encoding.UTF8;

                    var json = encoding.GetString(content);

                    if (statusCode == HttpStatusCode.BadRequest)
                    {
                        var apiValidation = System.Text.Json.JsonSerializer.Deserialize<JsonProblemResult>(json);

                        result = new ApiErrorInfo
                        {
                            ErrorMessage = apiValidation.title,
                            ErrorCode = apiValidation.status,
                            Errors = apiValidation.errors
                        };
                    }
                    else
                    {
                        result = System.Text.Json.JsonSerializer.Deserialize<ApiErrorInfo>(json);
                    }
                }
            }
           
            return result;
        }

        private static bool IsJson(string? mediaType)
        {
            return mediaType == "application/problem+json" || mediaType == "application/json";
        }

        private class JsonProblemResult
        {
            public int status { get; set; }
            public string? title { get; set; }
            public Dictionary<string, List<string>>? errors { get; set; }

        }     
    }

    public class ApiErrorInfo
    {
        public string? ErrorMessage { get; internal set; }

        public string? StackTrace { get; internal set; }

        public int? ErrorCode { get; internal set; }

        public Dictionary<string, List<string>>? Errors { get; internal set; }

        [Pure]
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(ErrorMessage);

            if (Errors != null && Errors.Count > 0)
            {
                sb.AppendLine();
                sb.Append(Errors.Select(x => $"{x.Key}=[{ string.Join(", ", x.Value)}]")
                .Aggregate((x, y) => $"{x}.\r\n{y}"));
            }
            
            if(StackTrace!= null)
            {
                sb.AppendLine();
                sb.Append("StackTrace:" + StackTrace);
            }

            return sb.ToString();
        }

    }
}
