﻿using Cybtans.Entities;
using Cybtans.Refit;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Tests.Integrations
{
    public class ReadOnlyEntityTest : IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;        
        IReadOnlyEntityService _service;

        public ReadOnlyEntityTest(IntegrationFixture fixture)
        {
            _fixture = fixture;            
            _service = fixture.GetClient<IReadOnlyEntityService>();
        }
       
        [Fact]
        public async Task GetAll()
        {
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            Assert.True(result.TotalCount > 0);
            Assert.Equal(result.TotalCount, result.Items.Count);
        }

        [Fact]
        public async Task GetAllWithNoRole()
        {
            await _fixture.CreateTest()
                .UseRoles("no-admin")
                .RunAsync<IReadOnlyEntityService>(async service =>
                {
                    var exception = await Assert.ThrowsAsync<ApiException>(() => service.GetAll());
                    Assert.NotNull(exception);
                    Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
                });
        }      
    }
}
