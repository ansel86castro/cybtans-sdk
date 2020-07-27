using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException ve:
                    context.Result = new ObjectResult(ve.ValidationResult) { StatusCode = (int)HttpStatusCode.BadRequest };
                    context.ExceptionHandled = true;
                    break;
                case EntityNotFoundException en:
                    context.Result = new ObjectResult(new ValidationResult(en.Message)) { StatusCode = (int)HttpStatusCode.NotFound };
                    context.ExceptionHandled = true;
                    break;
                case ActionException actionException:
                    context.Result = new ObjectResult(new ValidationResult(actionException.Response.ToString()))
                    {
                        StatusCode = (int)actionException.StatusCode
                    };
                    context.ExceptionHandled = true;
                    break;
                default:
                    _logger.LogError(context.Exception, context.Exception.Message);
                    context.Result = new ObjectResult(new ValidationResult(context.Exception.Message)) { StatusCode = (int)HttpStatusCode.InternalServerError };
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}
