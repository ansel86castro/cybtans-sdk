using Cybtans.Refit;
using Cybtans.Tests.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Integrations
{
    public class ClientServiceTest: IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;       
        Clients.IClientServiceClient _service;

        public ClientServiceTest(IntegrationFixture fixture)
        {
            _fixture = fixture;            
            _service = fixture.GetClient<Clients.IClientServiceClient>();
        }

        [Fact]
        public async void GetClient()
        {
            await _fixture.CreateTest()
                .UseClaims(
                    new Claim("client_id", "D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                    new Claim("creator_id", "1"))
                .RunAsync<IClientServiceClient>(async client =>
                {
                    var result = await client.GetClient(Guid.Parse("D6E29710-B68F-4D2D-9471-273DECF9C4B7"));
                    Assert.NotNull(result);
                });
        }

        [Fact]
        public async void GetClient_RequestPolicyFail()
        {
            await _fixture.CreateTest()
                .UseClaims(
                    new Claim("client_id", "D6E29718-B68F-4D2D-9471-273DECF9C4B7"),
                    new Claim("creator_id", "1"))
                .RunAsync<IClientServiceClient>(async client =>
                {
                    var result = await Assert.ThrowsAsync<ApiException>(() => client.GetClient(Guid.Parse("D6E29710-B68F-4D2D-9471-273DECF9C4B7")));
                    Assert.NotNull(result);
                    Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
                });
        }

        [Fact]
        public async void GetClient_ResultPolicyFail()
        {
            await _fixture.CreateTest()
                .UseClaims(
                    new Claim("client_id", "D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                    new Claim("creator_id", "2"))
                .RunAsync<IClientServiceClient>(async client =>
                {
                    var result = await Assert.ThrowsAsync<ApiException>(() => client.GetClient(Guid.Parse("D6E29710-B68F-4D2D-9471-273DECF9C4B7")));
                    Assert.NotNull(result);
                    Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
                });
        }

        [Fact]
        public async void GetClientwithEmptyId()
        {
            await _fixture.CreateTest()
                .UseClaims(
                    new Claim("client_id", "D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                    new Claim("creator_id", "1"))
                .RunAsync<IClientServiceClient>(async client =>
                {
                    var result = await client.GetClient(Guid.Empty);
                    Assert.NotNull(result);
                });
        }
    }
}
