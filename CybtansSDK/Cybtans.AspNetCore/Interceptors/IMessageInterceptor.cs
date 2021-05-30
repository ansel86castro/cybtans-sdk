using System.Threading.Tasks;

namespace Cybtans.AspNetCore.Interceptors
{
    public interface IActionInterceptor
    {
        ValueTask Handle<T>(T msg, string action) where T : class;
    }
}
