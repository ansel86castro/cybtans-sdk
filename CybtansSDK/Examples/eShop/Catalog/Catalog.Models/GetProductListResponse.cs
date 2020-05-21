using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Catalog.Models
{
	public partial class GetProductListResponse : IReflectorMetadataProvider
	{
		private static readonly GetProductListResponseAccesor __accesor = new GetProductListResponseAccesor();
		
		public List<Product> Items {get; set;}
		
		public int Page {get; set;}
		
		public int TotalPages {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class GetProductListResponseAccesor : IReflectorMetadata
	{
		public const int Items = 1;
		public const int Page = 2;
		public const int TotalPages = 3;
		private readonly int[] _props = new []
		{
			Items,Page,TotalPages
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Items => "Items",
		       Page => "Page",
		       TotalPages => "TotalPages",
		
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
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Items => typeof(List<Product>),
		        Page => typeof(int),
		        TotalPages => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetProductListResponse obj = (GetProductListResponse)target;
		    return propertyCode switch
		    {
		        Items => obj.Items,
		        Page => obj.Page,
		        TotalPages => obj.TotalPages,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetProductListResponse obj = (GetProductListResponse)target;
		    switch (propertyCode)
		    {
		        case Items:  obj.Items = (List<Product>)value;break;
		        case Page:  obj.Page = (int)value;break;
		        case TotalPages:  obj.TotalPages = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
