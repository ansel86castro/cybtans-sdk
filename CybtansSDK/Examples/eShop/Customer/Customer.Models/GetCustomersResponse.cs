using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Customer.Models
{
	public partial class GetCustomersResponse : IReflectorMetadataProvider
	{
		private static readonly GetCustomersResponseAccesor __accesor = new GetCustomersResponseAccesor();
		
		public List<Customer> Items {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
		
		public static implicit operator GetCustomersResponse(List<Customer> items)
		{
			return new GetCustomersResponse { Items = items };
		}
	}
	
	
	public sealed class GetCustomersResponseAccesor : IReflectorMetadata
	{
		public const int Items = 1;
		private readonly int[] _props = new []
		{
			Items
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Items => "Items",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Items" => Items,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Items => typeof(List<Customer>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetCustomersResponse obj = (GetCustomersResponse)target;
		    return propertyCode switch
		    {
		        Items => obj.Items,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetCustomersResponse obj = (GetCustomersResponse)target;
		    switch (propertyCode)
		    {
		        case Items:  obj.Items = (List<Customer>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
