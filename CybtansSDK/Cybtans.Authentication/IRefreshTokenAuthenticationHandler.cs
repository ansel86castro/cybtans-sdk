using System.Security.Claims;

namespace Cybtans.Authentication
{
    public interface IRefreshTokenAuthenticationHandler
    {
        Task<ClaimsIdentity?> AuthenticateAsync(string refreshToken);

        Task<RefreshToken> AddAsync(RefreshToken refreshToken);
    }
}
