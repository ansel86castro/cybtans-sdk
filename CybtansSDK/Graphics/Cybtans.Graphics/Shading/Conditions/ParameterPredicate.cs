using Cybtans.Graphics.Models;
using System.Collections.Generic;

namespace Cybtans.Graphics.Shading
{
    public enum Operator
    {
        IsActive = 1,
        Equal = 2,
        NotEqual = 3,
        LessThan = 4,
        GratherThan = 5 ,
        LessThanEqual = 6,
        GreaterThanEqual = 7
    }

    
    public class ParameterPredicate
    {
        public Operator Op { get; set; }
      
        public string Parameter { get; set; }
        
        public object Value { get; set; }

        public static ParameterPredicate FromDto(ParameterPredicateDto dto)
        {
            if (dto == null)
                return null;

            return new ParameterPredicate
            {
                Op = (Operator)dto.Op,
                Parameter = dto.Parameter,
                Value = dto.Value
            };
        }

        public static ParameterPredicateDto ToDto(ParameterPredicate p)
        {
            if (p == null) return null;

            return new ParameterPredicateDto
            {
                Op = (int) p.Op,
                Parameter = p.Parameter,
                Value =p.Value
            };
        }
    }

    
   
}
