using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class UpdateSoftDeleteOrderRequest : IReflectorMetadataProvider
	{
		private static readonly UpdateSoftDeleteOrderRequestAccesor __accesor = new UpdateSoftDeleteOrderRequestAccesor();
		
		public Guid Id {get; set;}
		
		public SoftDeleteOrderDto Value {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UpdateSoftDeleteOrderRequestAccesor : IReflectorMetadata
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
		        Value => typeof(SoftDeleteOrderDto),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UpdateSoftDeleteOrderRequest obj = (UpdateSoftDeleteOrderRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Value => obj.Value,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UpdateSoftDeleteOrderRequest obj = (UpdateSoftDeleteOrderRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Value:  obj.Value = (SoftDeleteOrderDto)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
