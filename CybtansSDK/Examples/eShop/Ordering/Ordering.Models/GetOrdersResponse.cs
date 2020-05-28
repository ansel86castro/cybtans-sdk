using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Ordering.Models
{
	public partial class GetOrdersResponse : IReflectorMetadataProvider
	{
		private static readonly GetOrdersResponseAccesor __accesor = new GetOrdersResponseAccesor();
		
		public List<Order> Orders {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator GetOrdersResponse(List<Order> orders)
		{
			return new GetOrdersResponse { Orders = orders };
		}
	}
	
	
	public sealed class GetOrdersResponseAccesor : IReflectorMetadata
	{
		public const int Orders = 1;
		private readonly int[] _props = new []
		{
			Orders
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Orders => "Orders",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Orders" => Orders,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Orders => typeof(List<Order>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetOrdersResponse obj = (GetOrdersResponse)target;
		    return propertyCode switch
		    {
		        Orders => obj.Orders,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetOrdersResponse obj = (GetOrdersResponse)target;
		    switch (propertyCode)
		    {
		        case Orders:  obj.Orders = (List<Order>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
