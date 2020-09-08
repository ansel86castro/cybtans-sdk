using Cybtans.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccessTokenExtensions
    {
        public static void AddAuthenticatedHttpHandler(this IServiceCollection services)
        {
            services.TryAddTransient<HttpClientAuthorizationHandler>();
        }

        public static IServiceCollection AddAccessTokenManager(this IServiceCollection services, IConfiguration config)
        {
            TokenManagerOptions options = new TokenManagerOptions();
            config.Bind("TokenManagerOptions", options);
            services.TryAddSingleton(options);

            services.TryAddSingleton<IAccessTokenManager, AccessTokenManager>();
            services.TryAddTransient<TokenManagerAuthenticationHandler>();

            return services;
        }

        public static IServiceCollection AddAccessTokenManager(this IServiceCollection services, TokenManagerOptions options)
        {
            services.TryAddSingleton(options);

            services.TryAddSingleton<IAccessTokenManager, AccessTokenManager>();
            services.TryAddTransient<TokenManagerAuthenticationHandler>();

            return services;
        }
    }
}
