using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class UpdateCustomerEventRequest : IReflectorMetadataProvider
	{
		private static readonly UpdateCustomerEventRequestAccesor __accesor = new UpdateCustomerEventRequestAccesor();
		
		public Guid Id {get; set;}
		
		public CustomerEventDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UpdateCustomerEventRequestAccesor : IReflectorMetadata
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
		        Value => typeof(CustomerEventDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UpdateCustomerEventRequest obj = (UpdateCustomerEventRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UpdateCustomerEventRequest obj = (UpdateCustomerEventRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Value:  obj.Value = (CustomerEventDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
