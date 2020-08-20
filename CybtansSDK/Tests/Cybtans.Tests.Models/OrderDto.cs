using System;
using Cybtans.Serialization;
using System.Collections.Generic;

namespace Cybtans.Tests.Models
{
	public class OrderDto : IReflectorMetadataProvider
	{
		private static readonly OrderDtoAccesor __accesor = new OrderDtoAccesor();
		
		public string Description {get; set;}
		
		public Guid CustomerId {get; set;}
		
		public int OrderStateId {get; set;}
		
		public OrderTypeEnum OrderType {get; set;}
		
		public OrderTypeEnum? OrderTypeNullable {get; set;}
		
		public OrderStateDto OrderState {get; set;}
		
		public CustomerDto Customer {get; set;}
		
		public List<OrderItemDto> Items {get; set;}
		
		public Guid? TenantId {get; set;}
		
		public Guid Id {get; set;}
		
		public DateTime CreateDate {get; set;}
		
		public DateTime? UpdateDate {get; set;}
		
		public string Creator {get; set;}
		
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
		public const int OrderTypeNullable = 5;
		public const int OrderState = 6;
		public const int Customer = 7;
		public const int Items = 8;
		public const int TenantId = 9;
		public const int Id = 10;
		public const int CreateDate = 11;
		public const int UpdateDate = 12;
		public const int Creator = 13;
		private readonly int[] _props = new []
		{
			Description,CustomerId,OrderStateId,OrderType,OrderTypeNullable,OrderState,Customer,Items,TenantId,Id,CreateDate,UpdateDate,Creator
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
		       OrderTypeNullable => "OrderTypeNullable",
		       OrderState => "OrderState",
		       Customer => "Customer",
		       Items => "Items",
		       TenantId => "TenantId",
		       Id => "Id",
		       CreateDate => "CreateDate",
		       UpdateDate => "UpdateDate",
		       Creator => "Creator",
		
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
		        "OrderTypeNullable" => OrderTypeNullable,
		        "OrderState" => OrderState,
		        "Customer" => Customer,
		        "Items" => Items,
		        "TenantId" => TenantId,
		        "Id" => Id,
		        "CreateDate" => CreateDate,
		        "UpdateDate" => UpdateDate,
		        "Creator" => Creator,
		
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
		        OrderTypeNullable => typeof(OrderTypeEnum?),
		        OrderState => typeof(OrderStateDto),
		        Customer => typeof(CustomerDto),
		        Items => typeof(List<OrderItemDto>),
		        TenantId => typeof(Guid?),
		        Id => typeof(Guid),
		        CreateDate => typeof(DateTime),
		        UpdateDate => typeof(DateTime?),
		        Creator => typeof(string),
		
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
		        OrderTypeNullable => obj.OrderTypeNullable,
		        OrderState => obj.OrderState,
		        Customer => obj.Customer,
		        Items => obj.Items,
		        TenantId => obj.TenantId,
		        Id => obj.Id,
		        CreateDate => obj.CreateDate,
		        UpdateDate => obj.UpdateDate,
		        Creator => obj.Creator,
		
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
		        case OrderTypeNullable:  obj.OrderTypeNullable = (OrderTypeEnum?)value;break;
		        case OrderState:  obj.OrderState = (OrderStateDto)value;break;
		        case Customer:  obj.Customer = (CustomerDto)value;break;
		        case Items:  obj.Items = (List<OrderItemDto>)value;break;
		        case TenantId:  obj.TenantId = (Guid?)value;break;
		        case Id:  obj.Id = (Guid)value;break;
		        case CreateDate:  obj.CreateDate = (DateTime)value;break;
		        case UpdateDate:  obj.UpdateDate = (DateTime?)value;break;
		        case Creator:  obj.Creator = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
