using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Services
{
    public class ValidationResult
    {
        public string ErrorMessage { get; set; }

        public Dictionary<string, List<string>> Errors { get; private set; }

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
                Errors.Add(member, new List<string>());
            }
            Errors[member].Add(error);            
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
