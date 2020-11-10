using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class DeleteOrderRequest : IReflectorMetadataProvider
	{
		private static readonly DeleteOrderRequestAccesor __accesor = new DeleteOrderRequestAccesor();
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator DeleteOrderRequest(Guid id)
		{
			return new DeleteOrderRequest { Id = id };
		}
	}
	
	
	internal sealed class DeleteOrderRequestAccesor : IReflectorMetadata
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
		    DeleteOrderRequest obj = (DeleteOrderRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    DeleteOrderRequest obj = (DeleteOrderRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
