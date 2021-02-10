using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Graphics.Models
{
	public partial class PredicateProgramDto : IReflectorMetadataProvider
	{
		private static readonly PredicateProgramDtoAccesor __accesor = new PredicateProgramDtoAccesor();
		
		public List<ParameterPredicateDto> AndConditions {get; set;}
		
		public List<ParameterPredicateDto> OrConditions {get; set;}
		
		public ParameterPredicateDto Condition {get; set;}
		
		[Required]
		public string Program {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class PredicateProgramDtoAccesor : IReflectorMetadata
	{
		public const int AndConditions = 1;
		public const int OrConditions = 2;
		public const int Condition = 3;
		public const int Program = 4;
		private readonly int[] _props = new []
		{
			AndConditions,OrConditions,Condition,Program
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       AndConditions => "AndConditions",
		       OrConditions => "OrConditions",
		       Condition => "Condition",
		       Program => "Program",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "AndConditions" => AndConditions,
		        "OrConditions" => OrConditions,
		        "Condition" => Condition,
		        "Program" => Program,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        AndConditions => typeof(List<ParameterPredicateDto>),
		        OrConditions => typeof(List<ParameterPredicateDto>),
		        Condition => typeof(ParameterPredicateDto),
		        Program => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    PredicateProgramDto obj = (PredicateProgramDto)target;
		    return propertyCode switch
		    {
		        AndConditions => obj.AndConditions,
		        OrConditions => obj.OrConditions,
		        Condition => obj.Condition,
		        Program => obj.Program,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    PredicateProgramDto obj = (PredicateProgramDto)target;
		    switch (propertyCode)
		    {
		        case AndConditions:  obj.AndConditions = (List<ParameterPredicateDto>)value;break;
		        case OrConditions:  obj.OrConditions = (List<ParameterPredicateDto>)value;break;
		        case Condition:  obj.Condition = (ParameterPredicateDto)value;break;
		        case Program:  obj.Program = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
