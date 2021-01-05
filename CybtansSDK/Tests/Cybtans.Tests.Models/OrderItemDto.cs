using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class OrderItemDto : IReflectorMetadataProvider
	{
		private static readonly OrderItemDtoAccesor __accesor = new OrderItemDtoAccesor();
		
		public string ProductName {get; set;}
		
		public float Price {get; set;}
		
		public float Discount {get; set;}
		
		public Guid OrderId {get; set;}
		
		public Guid Id {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class OrderItemDtoAccesor : IReflectorMetadata
	{
		public const int ProductName = 1;
		public const int Price = 2;
		public const int Discount = 3;
		public const int OrderId = 4;
		public const int Id = 5;
		private readonly int[] _props = new []
		{
			ProductName,Price,Discount,OrderId,Id
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       ProductName => "ProductName",
		       Price => "Price",
		       Discount => "Discount",
		       OrderId => "OrderId",
		       Id => "Id",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "ProductName" => ProductName,
		        "Price" => Price,
		        "Discount" => Discount,
		        "OrderId" => OrderId,
		        "Id" => Id,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        ProductName => typeof(string),
		        Price => typeof(float),
		        Discount => typeof(float),
		        OrderId => typeof(Guid),
		        Id => typeof(Guid),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    OrderItemDto obj = (OrderItemDto)target;
		    return propertyCode switch
		    {
		        ProductName => obj.ProductName,
		        Price => obj.Price,
		        Discount => obj.Discount,
		        OrderId => obj.OrderId,
		        Id => obj.Id,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    OrderItemDto obj = (OrderItemDto)target;
		    switch (propertyCode)
		    {
		        case ProductName:  obj.ProductName = (string)value;break;
		        case Price:  obj.Price = (float)value;break;
		        case Discount:  obj.Discount = (float)value;break;
		        case OrderId:  obj.OrderId = (Guid)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
