using System.Security.Claims;

namespace Cybtans.Services
{
    public interface ICurrentUserProvider
    {
        ClaimsPrincipal User { get; }
        string UserId { get; }
        string UserName { get; }
    }
}