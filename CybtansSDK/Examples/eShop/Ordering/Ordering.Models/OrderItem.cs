using System;
using Cybtans.Serialization;

namespace Ordering.Models
{
	public partial class OrderItem : IReflectorMetadataProvider
	{
		private static readonly OrderItemAccesor __accesor = new OrderItemAccesor();
		
		public int Id {get; set;}
		
		public string Name {get; set;}
		
		public float? Price {get; set;}
		
		public float Discount {get; set;}

		public Guid OrderId { get; set; }
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class OrderItemAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		public const int Price = 4;
		public const int Discount = 5;
		private readonly int[] _props = new []
		{
			Id,Name,Price,Discount
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Name => "Name",
		       Price => "Price",
		       Discount => "Discount",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Name" => Name,
		        "Price" => Price,
		        "Discount" => Discount,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(int),
		        Name => typeof(string),
		        Price => typeof(float?),
		        Discount => typeof(float),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    OrderItem obj = (OrderItem)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Name => obj.Name,
		        Price => obj.Price,
		        Discount => obj.Discount,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    OrderItem obj = (OrderItem)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (int)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case Price:  obj.Price = (float?)value;break;
		        case Discount:  obj.Discount = (float)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
