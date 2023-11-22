using Cybtans.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService = Cybtans.Authentication.AuthenticationService;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Cybtans.Authorization.Tests
{
    public class ValidateTokenTest: IClassFixture<LocalDirectoryFixture>
    {
        private LocalDirectoryRepository _repository;
        private AuthenticationService _authenticationService;
        public ValidateTokenTest(LocalDirectoryFixture fixture) 
        {
            _repository = fixture.Repo;
            _authenticationService = AuthenticationServiceFactory.Create(_repository);
        }

        [Fact]
        public async Task ValidateToken()
        {
            var result = await _authenticationService.GetToken(new TokenRequest
            {
                Username = "test",
                Password = "test",
                GrandType = "password"
            });
            Assert.NotNull(result);
            Assert.NotNull(result.RefreshToken);
            Assert.NotEmpty(result.AccessToken);

            var jwt = new JwtSecurityTokenHandler();
            var cert = await _repository.GetDefaultCertificate();
            Assert.NotNull(cert);

            var validationParameters = new TokenValidationParameters
            {
                ValidateTokenReplay = false,
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,

                ValidAudience = "test",
                ValidIssuer = "http://test.com",
                IssuerSigningKey = new RsaSecurityKey(cert.Certificate.GetRSAPublicKey()),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5.0)
            };

            var tokenPrincipal = jwt.ValidateToken(result.AccessToken, validationParameters, out var securityToken);
            var jwtSeccurityToken  = (JwtSecurityToken)securityToken;

            Assert.NotNull(tokenPrincipal);
            Assert.NotNull(jwtSeccurityToken);
            Assert.True(tokenPrincipal.Identity.IsAuthenticated);
            Assert.Equal("1",tokenPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            Assert.Equal("uniquename", tokenPrincipal.FindFirstValue(ClaimTypes.Name));
            Assert.Equal("test", tokenPrincipal.FindFirstValue(ClaimTypes.Role));            


            jwt.ValidateToken(result.RefreshToken, validationParameters, out var refreshsecurityToken);
            Assert.NotNull(refreshsecurityToken);


        }

    }
}
