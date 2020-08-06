using Cybtans.Refit;
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
            if (context.Exception is ApiException apiException)
            {
                ErrorInfo result = apiException.ToErrorInfo();
                context.Result = new ObjectResult(result) { StatusCode = (int)apiException.StatusCode };
                context.ExceptionHandled = true;
            }
        }
    }


}
