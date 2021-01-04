using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CreateCustomerEventRequest : IReflectorMetadataProvider
	{
		private static readonly CreateCustomerEventRequestAccesor __accesor = new CreateCustomerEventRequestAccesor();
		
		public CustomerEventDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator CreateCustomerEventRequest(CustomerEventDto value)
		{
			return new CreateCustomerEventRequest { Value = value };
		}
	}
	
	
	public sealed class CreateCustomerEventRequestAccesor : IReflectorMetadata
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
		        Value => typeof(CustomerEventDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CreateCustomerEventRequest obj = (CreateCustomerEventRequest)target;
		    return propertyCode switch
		    {
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CreateCustomerEventRequest obj = (CreateCustomerEventRequest)target;
		    switch (propertyCode)
		    {
		        case Value:  obj.Value = (CustomerEventDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
