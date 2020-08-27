using Cybtans.Refit;
using Cybtans.Serialization;
using Cybtans.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.AspNetCore
{
    public static class Extensions
    {       

        public static ErrorInfo ToErrorInfo(this ApiException apiException)
        {
            ErrorInfo? result = null;

            if (apiException.Content != null)
            {
                if (apiException.ContentHeaders?.ContentType?.MediaType == BinarySerializer.MEDIA_TYPE)
                {                    
                    if (apiException.ContentHeaders.ContentType.CharSet == BinarySerializer.DefaultEncoding.WebName)
                    {
                        result = BinaryConvert.Deserialize<ValidationResult>(apiException.Content);
                    }
                    else
                    {
                        var encoding = Encoding.GetEncoding(apiException.ContentHeaders.ContentType.CharSet);
                        var serializer = new BinarySerializer(encoding);
                        result = serializer.Deserialize<ValidationResult>(apiException.Content);
                    }
                }
                else if (IsJson(apiException.ContentHeaders?.ContentType?.MediaType))
                {
                    var encoding = apiException.ContentHeaders?.ContentType?.CharSet != null ?
                        Encoding.GetEncoding(apiException.ContentHeaders.ContentType.CharSet) :
                        Encoding.UTF8;

                    var json = encoding.GetString(apiException.Content);
                    if (apiException.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var apiValidation = System.Text.Json.JsonSerializer.Deserialize<FluentValidationResult>(json);

                        result = new ValidationResult(apiValidation.title, apiValidation.errors)
                        {
                            ErrorCode = (int)apiException.StatusCode
                        };
                    }
                    else
                    {
                        result = System.Text.Json.JsonSerializer.Deserialize<ValidationResult>(json);
                    }
                }
            }

            if (result == null)
            {
                result = new ErrorInfo(apiException.Message) { ErrorCode = (int) apiException.StatusCode };
            }

            return result;
        }

        private static bool IsJson(string? mediaType)
        {
            return mediaType == "application/problem+json" || mediaType == "application/json";
        }

        internal static string GetFullErrorMessage(this Exception exception)
        {
            var sb = new StringBuilder(exception.Message);

            var innerException = exception.InnerException;
            while(innerException != null)
            {
                sb.AppendLine();
                sb.AppendLine("----Inner Exception---");
                sb.AppendLine(innerException.Message);

                innerException = innerException.InnerException;
            }

            return sb.ToString();
        }

        public class FluentValidationResult
        {
            public int status { get; set; }
            public string? title { get; set; }
            public Dictionary<string, List<string>>? errors { get; set; }

        }
    }
}
