using System;
using Cybtans.Serialization;

namespace Cybtans.Graphics.Models
{
	public partial class ParameterPredicateDto : IReflectorMetadataProvider
	{
		private static readonly ParameterPredicateDtoAccesor __accesor = new ParameterPredicateDtoAccesor();
		
		public int Op {get; set;}
		
		public string Parameter {get; set;}
		
		public object Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class ParameterPredicateDtoAccesor : IReflectorMetadata
	{
		public const int Op = 1;
		public const int Parameter = 2;
		public const int Value = 3;
		private readonly int[] _props = new []
		{
			Op,Parameter,Value
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Op => "Op",
		       Parameter => "Parameter",
		       Value => "Value",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Op" => Op,
		        "Parameter" => Parameter,
		        "Value" => Value,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Op => typeof(int),
		        Parameter => typeof(string),
		        Value => typeof(object),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    ParameterPredicateDto obj = (ParameterPredicateDto)target;
		    return propertyCode switch
		    {
		        Op => obj.Op,
		        Parameter => obj.Parameter,
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    ParameterPredicateDto obj = (ParameterPredicateDto)target;
		    switch (propertyCode)
		    {
		        case Op:  obj.Op = (int)value;break;
		        case Parameter:  obj.Parameter = (string)value;break;
		        case Value:  obj.Value = (object)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
