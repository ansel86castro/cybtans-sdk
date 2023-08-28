using Cybtans.Common;
using Cybtans.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cybtans.AspNetCore
{
    public class UpstreamExceptionFilter : IExceptionFilter
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
