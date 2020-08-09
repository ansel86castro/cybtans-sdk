using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Cybtans.AspNetCore
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        ILogger<HttpResponseExceptionFilter> _logger;
        IHostEnvironment _env;

        public HttpResponseExceptionFilter(IHostEnvironment env, ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException ve:
                    if (_env.IsDevelopment())
                    {
                        ve.ValidationResult.StackTrace = context.Exception.StackTrace;                       
                    }
                    context.Result = new ObjectResult(ve.ValidationResult) 
                    { 
                        StatusCode = ve.ErrorCode 
                    };                    
                    context.ExceptionHandled = true;
                    break;
                case EntityNotFoundException en:
                    context.Result = new ObjectResult(new ValidationResult(en.Message)
                    {
                         ErrorCode = (int)HttpStatusCode.NotFound,
                         StackTrace = _env.IsDevelopment()?  en.StackTrace: null
                    }) 
                    { 
                        StatusCode = (int)HttpStatusCode.NotFound 
                    };
                    context.ExceptionHandled = true;
                    break;
                case CybtansException actionException:
                    context.Result = new ObjectResult(new ValidationResult(actionException.Message) 
                    { 
                        ErrorCode = actionException.ErrorCode, 
                        StackTrace = _env.IsDevelopment() ? actionException.StackTrace : null  })
                    {
                        StatusCode = (int)actionException.StatusCode
                    };
                    context.ExceptionHandled = true;
                    break;
                default:
                    _logger.LogError(context.Exception, context.Exception.Message);

                    context.Result = new ObjectResult(new ValidationResult(context.Exception.GetFullErrorMessage())
                    {
                        ErrorCode = (int)HttpStatusCode.InternalServerError,
                        StackTrace = _env.IsDevelopment() ? context.Exception.StackTrace : null
                    })
                    { StatusCode = (int)HttpStatusCode.InternalServerError };

                    context.ExceptionHandled = true;                    
                    break;
            }
        }
    }
}
