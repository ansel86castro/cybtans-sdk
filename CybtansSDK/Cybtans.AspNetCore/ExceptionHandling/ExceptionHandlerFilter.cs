using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Cybtans.AspNetCore
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        ILogger<HttpResponseExceptionFilter> _logger;
        IHostEnvironment _env;
        bool _showDetails;

        public HttpResponseExceptionFilter(IHostEnvironment env, ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
            _env = env;
            _showDetails = !_env.IsProduction();           
        }
        

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException ve:
                    if (_showDetails)
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
                         StackTrace = _showDetails ?  en.StackTrace: null
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
                        StackTrace = _showDetails ? actionException.StackTrace : null  })
                    {
                        StatusCode = (int)actionException.StatusCode
                    };
                    context.ExceptionHandled = true;
                    break;
                case NotImplementedException notImplemented:
                    _logger.LogError(context.Exception, context.Exception.Message);

                    context.Result = new ObjectResult(new ValidationResult(_showDetails ? notImplemented.Message : "Operation not supported")
                    {
                        ErrorCode = (int)HttpStatusCode.NotImplemented,
                        StackTrace = _showDetails ? notImplemented.StackTrace : null
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotImplemented
                    };
                    context.ExceptionHandled = true;
                    break;
                case ArgumentException argumentException:                    
                    context.Result = new ObjectResult(new ValidationResult(_showDetails? argumentException.Message: "An error has occurred")
                    {
                        ErrorCode = (int)HttpStatusCode.BadRequest,
                        StackTrace = _showDetails ? argumentException.StackTrace : null
                    })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                    context.ExceptionHandled = true;
                    break;                    
                default:
                    _logger.LogError(context.Exception, context.Exception.Message);

                    context.Result = new ObjectResult(new ValidationResult(_showDetails ? context.Exception.GetFullErrorMessage() : "An error has occurred")
                    {
                        ErrorCode = (int)HttpStatusCode.InternalServerError,
                        StackTrace = _showDetails ? context.Exception.StackTrace : null
                    })
                    { StatusCode = (int)HttpStatusCode.InternalServerError };

                    context.ExceptionHandled = true;                    
                    break;
            }
        }
    }
}
