using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Services
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {

        }

        public ValidationException(string message)
            : base(message)
        {
            ValidationResult = new ValidationResult(message);
        }

        public ValidationException(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public ValidationException(ValidationResult validationResult, string message, Exception innerException)
            : base(message, innerException)
        {
            ValidationResult = validationResult;
        }
        public ValidationException AddError(string member, string error)
        {
            if (ValidationResult == null)
                ValidationResult = new ValidationResult();

            ValidationResult.AddError(member, error);
            return this;
        }

        public ValidationResult ValidationResult { get; private set; }

    }
}
