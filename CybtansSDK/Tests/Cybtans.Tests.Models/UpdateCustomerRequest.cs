using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class UpdateCustomerRequest : IReflectorMetadataProvider
	{
		private static readonly UpdateCustomerRequestAccesor __accesor = new UpdateCustomerRequestAccesor();
		
		public Guid Id {get; set;}
		
		public CustomerDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UpdateCustomerRequestAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Value = 2;
		private readonly int[] _props = new []
		{
			Id,Value
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Value => "Value",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Value" => Value,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        Value => typeof(CustomerDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UpdateCustomerRequest obj = (UpdateCustomerRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UpdateCustomerRequest obj = (UpdateCustomerRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Value:  obj.Value = (CustomerDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
