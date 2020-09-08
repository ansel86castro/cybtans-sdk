using System.Threading.Tasks;

namespace Cybtans.AspNetCore
{
    public interface IAccessTokenManager
    {
        void ClearToken();
        ValueTask<string> GetToken();
    }
}