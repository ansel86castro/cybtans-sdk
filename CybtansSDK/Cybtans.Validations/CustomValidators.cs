using FluentValidation;
using System;

namespace Cybtans.Validations
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> MustBeGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(v => Guid.TryParse(v, out _)).WithMessage("Invalid guid format");
        }
    }
}
