using Cybtans.AspNetCore.Interceptors;
using Cybtans.Validations;
using System.Threading.Tasks;

namespace Cybtans.Tests.RestApi
{
    public class ValidatorActionInterceptor : IActionInterceptor
    {
        private readonly IValidatorProvider _validatorProvider ;

        public ValidatorActionInterceptor(IValidatorProvider validatorProvider)
        {
            _validatorProvider  = validatorProvider;
        }

        public ValueTask Handle<T>(T msg, string action) where T:class
        {
            return _validatorProvider.ValidateAsync(msg);
        }
    }
}
