using Cybtans.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Authorization.Tests
{
    public class AuthenticationServiceTest : IClassFixture<LocalDirectoryFixture>
    {
        private AuthenticationService _service;

        public AuthenticationServiceTest(LocalDirectoryFixture fixture)
        {
            _service = AuthenticationServiceFactory.Create(fixture.Repo);
        }

        [Fact]
        public async Task GetJWTK()
        {
            var result = await _service.GetJwtkSet();
            Assert.NotNull( result );
            Assert.NotEmpty(result);

            var key = result.First();
            Assert.NotNull( key );
            Assert.NotNull(key.E);
            Assert.NotNull(key.N);
            Assert.NotNull(key.Kid);
        }

        [Fact]
        public async Task UserAuthentication()
        {
            var token = await _service.GetToken(new TokenRequest
            {
                Username = "test",
                Password = "test",
                GrandType = "password"                
            });
            Assert.NotNull(token);
            Assert.NotNull(token.RefreshToken);
            Assert.NotEmpty(token.AccessToken);
            
        }


        [Fact]
        public async Task RefreshTokeAuthentication()
        {
            var result = await _service.GetToken(new TokenRequest
            {
                Username = "test",
                Password = "test",
                GrandType = GrantTypes.Password
            });

            Assert.NotNull(result.RefreshToken);
            result = await _service.GetToken(new TokenRequest
            {
                RefreshToken = result.RefreshToken,
                GrandType= GrantTypes.RefreshToken

            });
            Assert.NotEmpty(result.AccessToken);
            Assert.NotNull(result.RefreshToken);            
        }

       
    }
}
