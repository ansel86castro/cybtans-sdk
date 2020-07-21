using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Cybtans.AspNetCore
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {           
            switch (context.Exception)
            {
                case ValidationException ve:
                    context.Result = new ObjectResult(ve.ValidationResult) { StatusCode = (int)HttpStatusCode.BadRequest };
                    context.ExceptionHandled = true;
                    break;
                case ActionException actionException:
                    context.Result = new ObjectResult(new { ErrorMessage = actionException.Message, Response = actionException.Response }) 
                    { 
                        StatusCode = (int)actionException.StatusCode
                    };
                    context.ExceptionHandled = true;                                  
                    break;
            }
        }
    }
}
