using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using Cybtans.Tests.Services;
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
            _service = fixture.GetClient<AuthenticationServiceClient>(useJson :true);
        }

        [Fact]
        public async Task Login()
        {
            var response = await _service.Login(new LoginRequest
            {
                 Username = "admin",
                 Password = "admin"
            });

            Assert.NotNull(response);
            Assert.NotEmpty(response.Token);
        }

        [Fact]
        public async Task LoginBinary()
        {
            var client = _fixture.GetClient<AuthenticationServiceClient>();
            var response = await client.Login(new LoginRequest
            {
                Username = "admin",
                Password = "admin"
            });

            Assert.NotNull(response);
            Assert.NotEmpty(response.Token);
        }
    }
}
