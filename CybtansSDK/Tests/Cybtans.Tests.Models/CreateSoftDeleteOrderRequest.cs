using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CreateSoftDeleteOrderRequest : IReflectorMetadataProvider
	{
		private static readonly CreateSoftDeleteOrderRequestAccesor __accesor = new CreateSoftDeleteOrderRequestAccesor();
		
		public SoftDeleteOrderDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator CreateSoftDeleteOrderRequest(SoftDeleteOrderDto value)
		{
			return new CreateSoftDeleteOrderRequest { Value = value };
		}
	}
	
	
	public sealed class CreateSoftDeleteOrderRequestAccesor : IReflectorMetadata
	{
		public const int Value = 1;
		private readonly int[] _props = new []
		{
			Value
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Value => "Value",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Value" => Value,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Value => typeof(SoftDeleteOrderDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CreateSoftDeleteOrderRequest obj = (CreateSoftDeleteOrderRequest)target;
		    return propertyCode switch
		    {
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CreateSoftDeleteOrderRequest obj = (CreateSoftDeleteOrderRequest)target;
		    switch (propertyCode)
		    {
		        case Value:  obj.Value = (SoftDeleteOrderDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
