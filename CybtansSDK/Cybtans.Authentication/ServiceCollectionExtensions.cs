using Cybtans.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cybtans.Services.Auth
{
    public class AuthenticationBuilder
    {
        private readonly IServiceCollection _services;
        internal bool _useRefreshToken;
        
        internal AuthenticationBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public AuthenticationOptions Options { get; set; } = new AuthenticationOptions();      

        public PasswordHasherOptions PasswordHasherOptions { get; set; } = new PasswordHasherOptions();

        public void UsePasswordAuthentication<T>() where T: class, IPasswordAuthenticationHandler
        {
            _services.AddScoped<IPasswordAuthenticationHandler, T>();
        }

        public void UseRefreshoTokenAuthentication<T>() where T : class, IRefreshTokenAuthenticationHandler
        {
            _services.AddScoped<IRefreshTokenAuthenticationHandler, T>();
            _useRefreshToken = true;
        }

        public void UseClientCredentialAuthentication<T>() where T : class, IClientCredentialAuthenticationHandler
        {
            _services.AddScoped<IClientCredentialAuthenticationHandler, T>();
        }

        public void UseJtwkRepository<T>() where T:class, ICertificateRepository
        {
            _services.AddSingleton<ICertificateRepository, T>();
        }

        public void UseJtwkRepository<T>(T repository) where T : class, ICertificateRepository
        {
            _services.AddSingleton<ICertificateRepository>(repository);
        }

        public void UseRefreshTokenRepository<T>(T repository) where T: class, IRefreshTokenRepository
        {
            _services.AddScoped<IRefreshTokenRepository, T>();
        }

    }

    public static class ServiceCollectionExtensions
    {        
        public static void AddAuthenticationService(this IServiceCollection services, Action<AuthenticationBuilder> configure)
        {
            var builder = new AuthenticationBuilder(services);
            configure(builder);
            if (builder._useRefreshToken)
            {
                builder.Options.EnableRefreshToken = builder._useRefreshToken;
            }

            services.AddSingleton(builder.Options);
            services.AddSingleton(builder.PasswordHasherOptions);
            services.AddSingleton<IPasswordHasher, PasswordHasher>();           
            services.AddScoped<AuthenticationService>();            
        }
    }
}
