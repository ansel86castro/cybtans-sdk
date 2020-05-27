using System;
using Cybtans.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Models
{
	public partial class Order : IReflectorMetadataProvider
	{
		private static readonly OrderAccesor __accesor = new OrderAccesor();
		
		public Guid Id {get; set;}
		
		[Required]
		public string Name {get; set;}
		
		public string Description {get; set;}
		
		public DateTime CreateDate {get; set;}
		
		public float SubTotal {get; set;}
		
		public float Tax {get; set;}
		
		public float Total {get; set;}
		
		public int UserId {get; set;}
		
		public List<OrderItem> Items {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class OrderAccesor : IReflectorMetadata
	{
		public const int Id = 1;
		public const int Name = 2;
		public const int Description = 3;
		public const int CreateDate = 4;
		public const int SubTotal = 5;
		public const int Tax = 6;
		public const int Total = 7;
		public const int UserId = 8;
		public const int Items = 9;
		private readonly int[] _props = new []
		{
			Id,Name,Description,CreateDate,SubTotal,Tax,Total,UserId,Items
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Id => "Id",
		       Name => "Name",
		       Description => "Description",
		       CreateDate => "CreateDate",
		       SubTotal => "SubTotal",
		       Tax => "Tax",
		       Total => "Total",
		       UserId => "UserId",
		       Items => "Items",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Id" => Id,
		        "Name" => Name,
		        "Description" => Description,
		        "CreateDate" => CreateDate,
		        "SubTotal" => SubTotal,
		        "Tax" => Tax,
		        "Total" => Total,
		        "UserId" => UserId,
		        "Items" => Items,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Id => typeof(Guid),
		        Name => typeof(string),
		        Description => typeof(string),
		        CreateDate => typeof(DateTime),
		        SubTotal => typeof(float),
		        Tax => typeof(float),
		        Total => typeof(float),
		        UserId => typeof(int),
		        Items => typeof(List<OrderItem>),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    Order obj = (Order)target;
		    return propertyCode switch
		    {
		        Id => obj.Id,
		        Name => obj.Name,
		        Description => obj.Description,
		        CreateDate => obj.CreateDate,
		        SubTotal => obj.SubTotal,
		        Tax => obj.Tax,
		        Total => obj.Total,
		        UserId => obj.UserId,
		        Items => obj.Items,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    Order obj = (Order)target;
		    switch (propertyCode)
		    {
		        case Id:  obj.Id = (Guid)value;break;
		        case Name:  obj.Name = (string)value;break;
		        case Description:  obj.Description = (string)value;break;
		        case CreateDate:  obj.CreateDate = (DateTime)value;break;
		        case SubTotal:  obj.SubTotal = (float)value;break;
		        case Tax:  obj.Tax = (float)value;break;
		        case Total:  obj.Total = (float)value;break;
		        case UserId:  obj.UserId = (int)value;break;
		        case Items:  obj.Items = (List<OrderItem>)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
