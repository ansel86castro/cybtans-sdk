using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CreateOrderRequest : IReflectorMetadataProvider
	{
		private static readonly CreateOrderRequestAccesor __accesor = new CreateOrderRequestAccesor();
		
		public OrderDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator CreateOrderRequest(OrderDto value)
		{
			return new CreateOrderRequest { Value = value };
		}
	}
	
	
	public sealed class CreateOrderRequestAccesor : IReflectorMetadata
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
		        Value => typeof(OrderDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CreateOrderRequest obj = (CreateOrderRequest)target;
		    return propertyCode switch
		    {
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CreateOrderRequest obj = (CreateOrderRequest)target;
		    switch (propertyCode)
		    {
		        case Value:  obj.Value = (OrderDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
