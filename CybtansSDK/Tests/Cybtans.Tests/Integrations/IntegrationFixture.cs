using Cybtans.Refit;
using Cybtans.Serialization;
using Cybtans.Test.Domain.EF;
using Cybtans.Test.RestApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Cybtans.Tests.Entities.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;
using Xunit;
using System.Threading.Tasks;

namespace Cybtans.Tests.Integrations
{
    public class IntegrationFixture: WebApplicationFactory<Startup>, IAsyncLifetime
    {
        HttpClient _httpClient;

        public HttpClient Client => _httpClient;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AdventureContext>();

                    db.Database.EnsureCreated();

                    RepositoryFixture.Seed(db).Wait();
                }
            });
        }
        
        protected override void ConfigureClient(HttpClient client)
        {           
            client.DefaultRequestHeaders.Add("Accept", $"{BinarySerializer.MEDIA_TYPE}; charset={Encoding.UTF8.WebName}");           
            base.ConfigureClient(client);
        }
             
                   
        public T GetClient<T>(HttpClient httpClient = null)
          where T : class
        {
            httpClient ??= _httpClient;
            var settings = new RefitSettings();
            settings.ContentSerializer = new CybtansContentSerializer(settings.ContentSerializer);
            return RestService.For<T>(httpClient, settings);
        }

        private void CreatePrincipal()
        {
            if (Thread.CurrentPrincipal == null)
            {                
                var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);
                AppDomain.CurrentDomain.SetThreadPrincipal(principal);
            }
        }

        public Task InitializeAsync()
        {
            _httpClient = CreateDefaultClient(new HttpClientErrorHandler());
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
