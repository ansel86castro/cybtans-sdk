using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class CreateCustomerRequest : IReflectorMetadataProvider
	{
		private static readonly CreateCustomerRequestAccesor __accesor = new CreateCustomerRequestAccesor();
		
		public CustomerDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator CreateCustomerRequest(CustomerDto value)
		{
			return new CreateCustomerRequest { Value = value };
		}
	}
	
	
	public sealed class CreateCustomerRequestAccesor : IReflectorMetadata
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
		        Value => typeof(CustomerDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    CreateCustomerRequest obj = (CreateCustomerRequest)target;
		    return propertyCode switch
		    {
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    CreateCustomerRequest obj = (CreateCustomerRequest)target;
		    switch (propertyCode)
		    {
		        case Value:  obj.Value = (CustomerDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
