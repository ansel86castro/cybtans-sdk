using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CreateOrderStateRequest : IReflectorMetadataProvider
	{
		private static readonly CreateOrderStateRequestAccesor __accesor = new CreateOrderStateRequestAccesor();
		
		public OrderStateDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator CreateOrderStateRequest(OrderStateDto value)
		{
			return new CreateOrderStateRequest { Value = value };
		}
	}
	
	
	public sealed class CreateOrderStateRequestAccesor : IReflectorMetadata
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
		        Value => typeof(OrderStateDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CreateOrderStateRequest obj = (CreateOrderStateRequest)target;
		    return propertyCode switch
		    {
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CreateOrderStateRequest obj = (CreateOrderStateRequest)target;
		    switch (propertyCode)
		    {
		        case Value:  obj.Value = (OrderStateDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
