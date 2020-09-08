using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cybtans.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cybtans.AspNetCore
{    
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
            

        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("Exception Middleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(httpContext, e).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Request.Headers.ContainsKey("Accept") && context.Request.Headers["Accept"].FirstOrDefault() == BinarySerializer.MEDIA_TYPE)
            {                
                context.Response.ContentType = BinarySerializer.MEDIA_TYPE;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;                
                await context.Response.BodyWriter.WriteAsync(BinaryConvert.Serialize(
                    new
                    {
                        context.Response.StatusCode,
                        Message = $"Internal Server Error {exception.Message}",
                    }));
            }
            else
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    context.Response.StatusCode,
                    Message = $"Internal Server Error {exception.Message}",
                }));
            }
            
        }
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
