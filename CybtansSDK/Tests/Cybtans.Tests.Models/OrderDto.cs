using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Tests.Models
{
	public partial class OrderDto : IReflectorMetadataProvider
	{
		private static readonly OrderDtoAccesor __accesor = new OrderDtoAccesor();
		
		public string Description {get; set;}
		
		public Guid CustomerId {get; set;}
		
		public int OrderStateId {get; set;}
		
		public OrderTypeEnum OrderType {get; set;}
		
		public OrderStateDto OrderState {get; set;}
		
		public List<OrderItemDto> Items {get; set;}
		
		public Guid Id {get; set;}
		
		public DateTime? CreateDate {get; set;}
		
		public DateTime? UpdateDate {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class OrderDtoAccesor : IReflectorMetadata
	{
		public const int Description = 1;
		public const int CustomerId = 2;
		public const int OrderStateId = 3;
		public const int OrderType = 4;
		public const int OrderState = 5;
		public const int Items = 6;
		public const int Id = 7;
		public const int CreateDate = 8;
		public const int UpdateDate = 9;
		private readonly int[] _props = new []
		{
			Description,CustomerId,OrderStateId,OrderType,OrderState,Items,Id,CreateDate,UpdateDate
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       Description => "Description",
		       CustomerId => "CustomerId",
		       OrderStateId => "OrderStateId",
		       OrderType => "OrderType",
		       OrderState => "OrderState",
		       Items => "Items",
		       Id => "Id",
		       CreateDate => "CreateDate",
		       UpdateDate => "UpdateDate",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "Description" => Description,
		        "CustomerId" => CustomerId,
		        "OrderStateId" => OrderStateId,
		        "OrderType" => OrderType,
		        "OrderState" => OrderState,
		        "Items" => Items,
		        "Id" => Id,
		        "CreateDate" => CreateDate,
		        "UpdateDate" => UpdateDate,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        Description => typeof(string),
		        CustomerId => typeof(Guid),
		        OrderStateId => typeof(int),
		        OrderType => typeof(OrderTypeEnum),
		        OrderState => typeof(OrderStateDto),
		        Items => typeof(List<OrderItemDto>),
		        Id => typeof(Guid),
		        CreateDate => typeof(DateTime?),
		        UpdateDate => typeof(DateTime?),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    OrderDto obj = (OrderDto)target;
		    return propertyCode switch
		    {
		        Description => obj.Description,
		        CustomerId => obj.CustomerId,
		        OrderStateId => obj.OrderStateId,
		        OrderType => obj.OrderType,
		        OrderState => obj.OrderState,
		        Items => obj.Items,
		        Id => obj.Id,
		        CreateDate => obj.CreateDate,
		        UpdateDate => obj.UpdateDate,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    OrderDto obj = (OrderDto)target;
		    switch (propertyCode)
		    {
		        case Description:  obj.Description = (string)value;break;
		        case CustomerId:  obj.CustomerId = (Guid)value;break;
		        case OrderStateId:  obj.OrderStateId = (int)value;break;
		        case OrderType:  obj.OrderType = (OrderTypeEnum)value;break;
		        case OrderState:  obj.OrderState = (OrderStateDto)value;break;
		        case Items:  obj.Items = (List<OrderItemDto>)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		        case CreateDate:  obj.CreateDate = (DateTime?)value;break;
		        case UpdateDate:  obj.UpdateDate = (DateTime?)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
