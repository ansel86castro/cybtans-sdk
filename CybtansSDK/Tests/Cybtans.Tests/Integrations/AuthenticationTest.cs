using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cybtans.Tests.Integrations
{
    public  class AuthenticationTest : IClassFixture<IntegrationFixture>
    {
        IntegrationFixture _fixture;
        ITestOutputHelper _testOutputHelper;
        IAuthenticationService _service;

        public AuthenticationTest(IntegrationFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
            _service = fixture.GetClient<IAuthenticationService>();
        }

        [Fact]
        public async Task Login()
        {
            var response = await _service.Login(new LoginRequest
            {
                 Username = "admin",
                 Password ="admin"
            });

            Assert.NotNull(response);
            Assert.NotEmpty(response.Token);
        }
    }
}
