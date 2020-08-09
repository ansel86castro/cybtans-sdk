#nullable enable

namespace Cybtans.Services
{
    public class ValidationException : CybtansException
    {
        public ValidationException():base(System.Net.HttpStatusCode.BadRequest)
        {
            ValidationResult = new ValidationResult();            
        }

        public ValidationException(string message)
            : base(System.Net.HttpStatusCode.BadRequest, message)
        {
            ValidationResult = new ValidationResult(message);
        }

        public ValidationException(ValidationResult validationResult)
            :base(System.Net.HttpStatusCode.BadRequest, validationResult.ErrorMessage)
        {
            ValidationResult = validationResult;
        }

        public ValidationResult ValidationResult { get; }

        public ValidationException AddError(string member, string error)
        {          
            ValidationResult.AddError(member, error);
            return this;
        }     

    }
}
