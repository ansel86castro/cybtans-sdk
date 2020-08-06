using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class UpdateOrderStateRequest : IReflectorMetadataProvider
	{
		private static readonly UpdateOrderStateRequestAccesor __accesor = new UpdateOrderStateRequestAccesor();
		
		public int Id {get; set;}
		
		public OrderStateDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UpdateOrderStateRequestAccesor : IReflectorMetadata
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
		        Id => typeof(int),
		        Value => typeof(OrderStateDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UpdateOrderStateRequest obj = (UpdateOrderStateRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UpdateOrderStateRequest obj = (UpdateOrderStateRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (int)value;break;
		        case Value:  obj.Value = (OrderStateDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
