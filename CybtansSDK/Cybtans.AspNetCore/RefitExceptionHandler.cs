using Cybtans.Serialization;
using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.AspNetCore
{
    public class RefitExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is Refit.ApiException apiException)
            {
                object result = null;

                if (apiException.HasContent)
                {
                    if (apiException.ContentHeaders?.ContentType?.MediaType == BinarySerializer.MEDIA_TYPE)
                    {
                        var bytes = Encoding.UTF8.GetBytes(apiException.Content);                       
                        result = BinaryConvert.Deserialize<ValidationResult>(bytes);
                      
                    }
                    else if (apiException.ContentHeaders?.ContentType?.MediaType == "application/json" && apiException.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var apiValidation = System.Text.Json.JsonSerializer.Deserialize<FluentValidationResult>(apiException.Content);
                        result = new ValidationResult(apiValidation.title, apiValidation.errors);
                    }
                }

                if (result == null)
                {
                    result = new { ErrorMessage = apiException.Message };
                }

                context.Result = new ObjectResult(result)
                {
                    StatusCode = (int)apiException.StatusCode
                };
                context.ExceptionHandled = true;
            }
        }
    }

    public class FluentValidationResult
    {
        public int status { get; set; }
        public string title { get; set; }
        public Dictionary<string, List<string>> errors { get; set; }

    }
}
