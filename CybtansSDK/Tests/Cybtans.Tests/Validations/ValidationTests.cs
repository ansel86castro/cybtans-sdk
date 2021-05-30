using Cybtans.Services;
using Cybtans.Tests.Models;
using Cybtans.Tests.Services.Validations;
using Cybtans.Validations;
using Docker.DotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cybtans.Tests.Services
{
    public class ValidationTests
    {
        private IValidatorProvider _validatorProvider;

        public ValidationTests()
        {
            var services = new ServiceCollection();
            services.AddDefaultValidatorProvider(p =>
            {
                p.AddValidators(new TestCreateOrderValidator());
            });

            var sp = services.BuildServiceProvider();
            _validatorProvider =  sp.GetRequiredService<IValidatorProvider>();
        }

        [Fact]
        public void ValidateCreateOrderNoBody()
        {
            var order = new CreateOrderRequest();
            var ex = Assert.Throws<ValidationException>(() => _validatorProvider.Validate(order));
            Assert.NotNull(ex);
            Assert.NotNull(ex.ValidationResult);
            Assert.Single(ex.ValidationResult.Errors);
        }

        [Fact]
        public void ValidateCreateOrderEmptyBody()
        {
            var order = new CreateOrderRequest()
            {
                 Value = new OrderDto
                 {

                 }
            };
            var ex = Assert.Throws<ValidationException>(() => _validatorProvider.Validate(order));
            Assert.NotNull(ex);
            Assert.NotNull(ex.ValidationResult);
            Assert.NotEmpty(ex.ValidationResult.Errors);
        }
    }
}
