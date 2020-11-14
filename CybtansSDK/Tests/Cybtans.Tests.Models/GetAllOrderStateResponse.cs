using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Tests.Models
{
	public class GetAllOrderStateResponse : IReflectorMetadataProvider
	{
		private static readonly GetAllOrderStateResponseAccesor __accesor = new GetAllOrderStateResponseAccesor();
		
		public List<OrderStateDto> Items {get; set;}
		
		public long Page {get; set;}
		
		public long TotalPages {get; set;}
		
		public long TotalCount {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	internal sealed class GetAllOrderStateResponseAccesor : IReflectorMetadata
	{
		public const int Items = 1;
		public const int Page = 2;
		public const int TotalPages = 3;
		public const int TotalCount = 4;
		private readonly int[] _props = new []
		{
			Items,Page,TotalPages,TotalCount
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Items => "Items",
		       Page => "Page",
		       TotalPages => "TotalPages",
		       TotalCount => "TotalCount",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Items" => Items,
		        "Page" => Page,
		        "TotalPages" => TotalPages,
		        "TotalCount" => TotalCount,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Items => typeof(List<OrderStateDto>),
		        Page => typeof(long),
		        TotalPages => typeof(long),
		        TotalCount => typeof(long),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetAllOrderStateResponse obj = (GetAllOrderStateResponse)target;
		    return propertyCode switch
		    {
		        Items => obj.Items,
		        Page => obj.Page,
		        TotalPages => obj.TotalPages,
		        TotalCount => obj.TotalCount,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetAllOrderStateResponse obj = (GetAllOrderStateResponse)target;
		    switch (propertyCode)
		    {
		        case Items:  obj.Items = (List<OrderStateDto>)value;break;
		        case Page:  obj.Page = (long)value;break;
		        case TotalPages:  obj.TotalPages = (long)value;break;
		        case TotalCount:  obj.TotalCount = (long)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
