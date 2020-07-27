using Cybtans.Serialization;
using Cybtans.Services.Extensions;
using Cybtans.Test.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Services
{
    public  class ServiceCollectionTests
    {
        [Fact]
        public async Task AddServices()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddCybtansServices("Cybtans.Tests.Services.dll");

            using (var scope = services.BuildServiceProvider())
            {
                var srv = scope.GetRequiredService<ICustomerService>();

                var customer = await srv.GetCustomer(Guid.NewGuid());
                Assert.NotNull(customer);
            }
        }

        [Fact]
        public async Task AddClients()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddTransient<TestHandler>();

            services.AddClients("https://localhost:5000", "Cybtans.Tests.Services.dll", (builder, type) =>
                {
                    Assert.NotNull(builder);
                    Assert.NotNull(type);

                    builder.AddHttpMessageHandler<TestHandler>();
                });

            using (var scope = services.BuildServiceProvider())
            {
                var client = scope.GetRequiredService<ICustomerClient>();

                var customer = await client.GetCustomer(Guid.Parse("E9FC9439-497F-4170-A79B-380F255A1420"));
                Assert.NotNull(customer);
                Assert.Equal("Customer", customer.Name);
            }         
        }
    }

    public class TestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = new ByteArrayContent(BinaryConvert.Serialize(new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Customer"
                })),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(BinarySerializer.MEDIA_TYPE);            
            return Task.FromResult(response);
        }
    }
}
