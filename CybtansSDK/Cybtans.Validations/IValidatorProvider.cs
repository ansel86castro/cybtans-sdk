using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Cybtans.Validations
{
    public interface IValidatorProvider
    {
        bool TryGetValidator<T>(out IValidator<T> result) where T : class;

        public void Validate<T>(T request) where T : class
        {
            if (TryGetValidator<T>(out var validator) && validator != null)
            {
                Validate(request, validator);
            }
        }

        public ValueTask ValidateAsync<T>(T request) where T : class
        {
            if (TryGetValidator<T>(out var validator) && validator != null)
            {
               return ValidateAsync(request, validator);
            }

            return ValueTask.CompletedTask;
        }

        public void Validate<T>(T request, IValidator<T> validator) where T : class
        {
            ValidationResult results = validator.Validate(request);

            if (!results.IsValid && results.Errors.Any())
            {
                throw new Services.ValidationException(ToValidationResult(results.Errors));
            }
        }

        public async ValueTask ValidateAsync<T>(T request, IValidator<T> validator) where T : class
        {
            ValidationResult results = await validator.ValidateAsync(request).ConfigureAwait(false);

            if (!results.IsValid && results.Errors.Any())
            {
                throw new Services.ValidationException(ToValidationResult(results.Errors));
            }
        }

        private static Services.ValidationResult ToValidationResult(IEnumerable<ValidationFailure> failures)
        {
            Services.ValidationResult err = new Services.ValidationResult("One or more validation errors occurred.") { ErrorCode = 400 };
          
            if (failures?.Any() ?? false)
            {                
                foreach (var item in failures)
                {
                    err.AddError(item.PropertyName, item.ErrorMessage);                    
                }                           
            }

            return err;
        }
    }
    
}
