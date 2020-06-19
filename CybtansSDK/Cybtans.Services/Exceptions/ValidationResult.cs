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

        public List<MemberValidationResult> Members { get; set; } = new List<MemberValidationResult>();

        public ValidationResult()
        {
            Members = new List<MemberValidationResult>();
        }

        public ValidationResult(string errorMessage)
            : this()
        {
            ErrorMessage = errorMessage;
        }


        public ValidationResult(string errorMessage, MemberValidationResult[] members)
            : this(errorMessage)
        {
            Members.AddRange(members);
        }

        public ValidationResult AddError(string member, string error)
        {
            Members.Add(new MemberValidationResult { Member = member, ErrorMessage = error });
            return this;
        }


        public bool ContainsError
        {
            get
            {
                return Members != null && Members.Count > 0 || !string.IsNullOrWhiteSpace(ErrorMessage);
            }
        }

    }


    public class MemberValidationResult
    {
        public string Member { get; set; }

        public string ErrorMessage { get; set; }

        public MemberValidationResult() { }

        public MemberValidationResult(string member, string errorMessage)
        {
            Member = member;
            ErrorMessage = errorMessage;
        }
    }
}
