using Cybtans.Grpc;
using Grpc.AspNetCore.Server;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {              

        public static void AddValidations(this GrpcServiceOptions options)
        {
            options.Interceptors.Add<ValidationInterceptor>();
        }

        public static void ConfigureExceptionHandlingInterceptor(this IServiceCollection services , Action<ExceptionHandlerInterceptor.Options> configure)
        {
            services.Configure(configure);
        }

        public static void AddExceptionHandling(this GrpcServiceOptions options)
        {                        
            options.Interceptors.Add<ExceptionHandlerInterceptor>();
        }     
    }
}
