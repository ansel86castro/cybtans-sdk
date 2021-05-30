using System.Collections.Generic;

namespace Cybtans.Services
{

    public class ValidationResult:ErrorInfo
    {        
        public Dictionary<string, List<string>> Errors { get; set; }

        public ValidationResult()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public ValidationResult(string errorMessage): this()
        {
            ErrorMessage = errorMessage;
        }

        public ValidationResult(string errorMessage, Dictionary<string, List<string>> errors): this(errorMessage)
        {
            Errors = errors;
        }

        public ValidationResult AddError(string member, string error)
        { 
            if(!Errors.TryGetValue(member, out var list))
            {
                list = new List<string>();
                Errors.Add(member, list);
            }

            list.Add(error);            

            return this;
        }

        public bool HasErrors
        {
            get
            {
                return Errors != null && Errors.Count > 0 || !string.IsNullOrWhiteSpace(ErrorMessage);
            }
        }

    }
}
