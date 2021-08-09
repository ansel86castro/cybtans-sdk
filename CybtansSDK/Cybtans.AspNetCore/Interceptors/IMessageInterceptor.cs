using System.Threading.Tasks;

namespace Cybtans.AspNetCore.Interceptors
{
    public interface IMessageInterceptor
    {
        ValueTask Handle<T>(T msg) where T : class;

        ValueTask HandleResult<T>(T result) where T : class;

    }
}
