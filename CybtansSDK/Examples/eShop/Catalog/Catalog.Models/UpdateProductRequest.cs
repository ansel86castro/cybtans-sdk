using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Catalog.Models
{
	public partial class UpdateProductRequest : IReflectorMetadataProvider
	{
		private static readonly UpdateProductRequestAccesor __accesor = new UpdateProductRequestAccesor();
		
		public int Id {get; set;}
		
		public Product Product {get; set;}
		
		public Dictionary<string,object> Data {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class UpdateProductRequestAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Product = 2;
		public const int Data = 3;
		private readonly int[] _props = new []
		{
			Id,Product,Data
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Product => "Product",
		       Data => "Data",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Product" => Product,
		        "Data" => Data,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(int),
		        Product => typeof(Product),
		        Data => typeof(Dictionary<string,object>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    UpdateProductRequest obj = (UpdateProductRequest)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Product => obj.Product,
		        Data => obj.Data,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    UpdateProductRequest obj = (UpdateProductRequest)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (int)value;break;
		        case Product:  obj.Product = (Product)value;break;
		        case Data:  obj.Data = (Dictionary<string,object>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
