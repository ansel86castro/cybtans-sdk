using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class DeleteSoftDeleteOrderRequest : IReflectorMetadataProvider
	{
		private static readonly DeleteSoftDeleteOrderRequestAccesor __accesor = new DeleteSoftDeleteOrderRequestAccesor();
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator DeleteSoftDeleteOrderRequest(Guid id)
		{
			return new DeleteSoftDeleteOrderRequest { Id = id };
		}
	}
	
	
	public sealed class DeleteSoftDeleteOrderRequestAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		private readonly int[] _props = new []
		{
			Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    DeleteSoftDeleteOrderRequest obj = (DeleteSoftDeleteOrderRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    DeleteSoftDeleteOrderRequest obj = (DeleteSoftDeleteOrderRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
