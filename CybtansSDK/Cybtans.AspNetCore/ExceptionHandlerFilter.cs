using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Cybtans.AspNetCore
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        ILogger<HttpResponseExceptionFilter> _logger;
        IHostingEnvironment _env;

        public HttpResponseExceptionFilter(IHostingEnvironment env, ILogger<HttpResponseExceptionFilter> logger)
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
                        ve.ValidationResult.ErrorCode = (int)HttpStatusCode.BadRequest;
                    }
                    context.Result = new ObjectResult(ve.ValidationResult) { StatusCode = (int)HttpStatusCode.BadRequest };
                    context.ExceptionHandled = true;
                    break;
                case EntityNotFoundException en:
                    context.Result = new ObjectResult(new ValidationResult(en.Message)
                    {
                         ErrorCode = (int)HttpStatusCode.NotFound,
                          StackTrace = _env.IsDevelopment()?  en.StackTrace: null
                    }) { StatusCode = (int)HttpStatusCode.NotFound };
                    context.ExceptionHandled = true;
                    break;
                case ActionException actionException:
                    context.Result = new ObjectResult(new ValidationResult(actionException.Message) 
                    { 
                        ErrorCode = (int)actionException.StatusCode, 
                        StackTrace = _env.IsDevelopment() ? actionException.StackTrace : null  })
                    {
                        StatusCode = (int)actionException.StatusCode
                    };
                    context.ExceptionHandled = true;
                    break;
                default:
                    _logger.LogError(context.Exception, context.Exception.Message);
                    context.Result = new ObjectResult(new ValidationResult(context.Exception.GetFullErrorMessage())) { StatusCode = (int)HttpStatusCode.InternalServerError };
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}
