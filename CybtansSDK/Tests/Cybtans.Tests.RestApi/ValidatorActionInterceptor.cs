using Cybtans.AspNetCore.Interceptors;
using Cybtans.Validations;
using System.Threading.Tasks;

namespace Cybtans.Tests.RestApi
{
    public class ValidatorActionInterceptor : IMessageInterceptor
    {
        private readonly IValidatorProvider _validatorProvider ;

        public ValidatorActionInterceptor(IValidatorProvider validatorProvider)
        {
            _validatorProvider  = validatorProvider;
        }

        public ValueTask Handle<T>(T msg) where T:class
        {
            return _validatorProvider.ValidateAsync(msg);
        }
    }
}
