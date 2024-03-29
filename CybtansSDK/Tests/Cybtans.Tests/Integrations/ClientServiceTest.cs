﻿using System;
using System.Net;
using System.Security.Claims;
using Xunit;
using Cybtans.Common;
using Cybtans.Tests.Services;

namespace Cybtans.Tests.Integrations
{
    public class ClientServiceTest: IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;       
        IClientService _service;

        public ClientServiceTest(IntegrationFixture fixture)
        {
            _fixture = fixture;            
            _service = fixture.GetClient<IClientService>();
        }

        [Fact]
        public async void GetClient()
        {
            await _fixture.CreateTest()
                .UseClaims(
                    new Claim("client_id", "D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                    new Claim("creator_id", "1"))
                .RunAsync<IClientService>(async client =>
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
                .RunAsync<IClientService>(async client =>
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
                .RunAsync<IClientService>(async client =>
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
                .RunAsync<IClientService>(async client =>
                {
                    var result = await client.GetClient(Guid.Empty);
                    Assert.NotNull(result);
                });
        }
    }
}
