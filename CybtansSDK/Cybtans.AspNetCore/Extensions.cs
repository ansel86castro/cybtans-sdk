using Cybtans.Refit;
using Cybtans.Serialization;
using Cybtans.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
                    result = BinaryConvert.Deserialize<ValidationResult>(apiException.Content);
                }
                else if (apiException.ContentHeaders?.ContentType?.MediaType == "application/json" && apiException.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var encoding = Encoding.GetEncoding(apiException.ContentHeaders.ContentType.CharSet);
                    var json = encoding.GetString(apiException.Content);
                    var apiValidation = System.Text.Json.JsonSerializer.Deserialize<FluentValidationResult>(json);

                    result = new ValidationResult(apiValidation.title, apiValidation.errors) { ErrorCode = (int)apiException.StatusCode };
                }
            }

            if (result == null)
            {
                result = new ErrorInfo(apiException.Message) { ErrorCode = (int) apiException.StatusCode };
            }

            return result;
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
