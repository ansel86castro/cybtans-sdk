using System;
using Cybtans.Serialization;

namespace Ordering.Models
{
	public partial class GetOrderByUserRequest : IReflectorMetadataProvider
	{
		private static readonly GetOrderByUserRequestAccesor __accesor = new GetOrderByUserRequestAccesor();
		
		public int UserId {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator GetOrderByUserRequest(int userId)
		{
			return new GetOrderByUserRequest { UserId = userId };
		}
	}
	
	
	public sealed class GetOrderByUserRequestAccesor : IReflectorMetadata
	{
		public const int UserId = 1;
		private readonly int[] _props = new []
		{
			UserId
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       UserId => "UserId",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "UserId" => UserId,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        UserId => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetOrderByUserRequest obj = (GetOrderByUserRequest)target;
		    return propertyCode switch
		    {
		        UserId => obj.UserId,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetOrderByUserRequest obj = (GetOrderByUserRequest)target;
		    switch (propertyCode)
		    {
		        case UserId:  obj.UserId = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
