using Cybtans.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cybtans.Authorization.Tests
{
    public class AuthenticationServiceFactory
    {
        class PasswordHandler : IPasswordAuthenticationHandler
        {
            public Task<ClaimsIdentity?> AuthenticateAsync(string username, string password)
            {
                return Task.FromResult(new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.GivenName, "givenname"),
                    new Claim(JwtRegisteredClaimNames.FamilyName, "familiyname"),
                    new Claim(JwtRegisteredClaimNames.UniqueName, "uniquename"),
                    new Claim(JwtRegisteredClaimNames.Sub, "1"),
                    new Claim("role", "test"),
                    new Claim("tid", "1")
                }, "Bearer"));
            }
        }

        class RefreshTokenHandler : IRefreshTokenAuthenticationHandler
        {
            public Task<RefreshToken> AddAsync(RefreshToken refreshToken)
            {
                refreshToken.Id = Guid.NewGuid().ToString();
                return Task.FromResult(refreshToken);
            }

            public Task<ClaimsIdentity?> AuthenticateAsync(string refreshToken)
            {
                return Task.FromResult(new ClaimsIdentity(new Claim[]
               {
                       new Claim(JwtRegisteredClaimNames.Name, "test"),
                    new Claim(JwtRegisteredClaimNames.Sub, "1"),
                    new Claim("role", "test")
               }, "Bearer"));
            }
        }

        class ClientCredentialHandler : IClientCredentialAuthenticationHandler
        {
            public Task<ClaimsIdentity?> AuthenticateAsync(string clientId, string clientSecret, string hint = null)
            {
                throw new NotImplementedException();
            }
        }

        public static AuthenticationService Create(ICertificateRepository certificateRepository)
        {
            return new AuthenticationService(new AuthenticationOptions
            {
                Issuer = "http://test.com",
                Audience= "test",
                EnableRefreshToken = true,
                ExpirationTimeSeconds = 3600,
                RefreshTokenExpireTimeSeconds = 3600,
                
            },
          certificateRepository,
           new PasswordHandler(),
           new RefreshTokenHandler(),
           new ClientCredentialHandler()
           );
        }
    }
}
