using System.Threading.Tasks;

namespace Cybtans.AspNetCore.Interceptors
{
    public interface IMessageInterceptor
    {
        ValueTask Handle<T>(T msg) where T : class;
    }
}
