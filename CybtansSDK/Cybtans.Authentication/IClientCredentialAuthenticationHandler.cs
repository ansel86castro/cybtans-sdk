using System.Security.Claims;
using System.Threading.Tasks;

namespace Cybtans.Authentication
{
    public interface IClientCredentialAuthenticationHandler
    {
        public Task<ClaimsIdentity?> AuthenticateAsync(string clientId, string clientSecret, string hint = null);
    }
}
