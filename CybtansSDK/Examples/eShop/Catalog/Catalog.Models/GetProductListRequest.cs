using System;
using Cybtans.Serialization;

namespace Catalog.Models
{
	public partial class GetProductListRequest : IReflectorMetadataProvider
	{
		private static readonly GetProductListRequestAccesor __accesor = new GetProductListRequestAccesor();
		
		public string Filter {get; set;}
		
		public string Sort {get; set;}
		
		public int Page {get; set;}
		
		public int PageSize {get; set;} = 50;
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class GetProductListRequestAccesor : IReflectorMetadata
	{
		public const int Filter = 1;
		public const int Sort = 2;
		public const int Page = 3;
		public const int PageSize = 4;
		private readonly int[] _props = new []
		{
			Filter,Sort,Page,PageSize
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Filter => "Filter",
		       Sort => "Sort",
		       Page => "Page",
		       PageSize => "PageSize",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Filter" => Filter,
		        "Sort" => Sort,
		        "Page" => Page,
		        "PageSize" => PageSize,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Filter => typeof(string),
		        Sort => typeof(string),
		        Page => typeof(int),
		        PageSize => typeof(int),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    GetProductListRequest obj = (GetProductListRequest)target;
		    return propertyCode switch
		    {
		        Filter => obj.Filter,
		        Sort => obj.Sort,
		        Page => obj.Page,
		        PageSize => obj.PageSize,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    GetProductListRequest obj = (GetProductListRequest)target;
		    switch (propertyCode)
		    {
		        case Filter:  obj.Filter = (string)value;break;
		        case Sort:  obj.Sort = (string)value;break;
		        case Page:  obj.Page = (int)value;break;
		        case PageSize:  obj.PageSize = (int)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
