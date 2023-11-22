using System.Security.Claims;
using System.Threading.Tasks;

namespace Cybtans.Authentication
{
    public interface IPasswordAuthenticationHandler
    {
        Task<ClaimsIdentity?> AuthenticateAsync(string username, string password);
    }
}
