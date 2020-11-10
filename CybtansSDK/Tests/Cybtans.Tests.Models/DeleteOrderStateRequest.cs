using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public class DeleteOrderStateRequest : IReflectorMetadataProvider
	{
		private static readonly DeleteOrderStateRequestAccesor __accesor = new DeleteOrderStateRequestAccesor();
		
		public int Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator DeleteOrderStateRequest(int id)
		{
			return new DeleteOrderStateRequest { Id = id };
		}
	}
	
	
	internal sealed class DeleteOrderStateRequestAccesor : IReflectorMetadata
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
		        Id => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    DeleteOrderStateRequest obj = (DeleteOrderStateRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    DeleteOrderStateRequest obj = (DeleteOrderStateRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
