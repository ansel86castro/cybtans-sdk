using System;
using Cybtans.Serialization;

namespace Cybtans.Tests.Models
{
	public partial class OrderNotification : IReflectorMetadataProvider
	{
		private static readonly OrderNotificationAccesor __accesor = new OrderNotificationAccesor();
		
		public string UserId {get; set;}
		
		public string OrderId {get; set;}
		
		public string Msg {get; set;}
		
		public IReflectorMetadata GetAccesor()
		{
			return __accesor;
		}
	}
	
	
	public sealed class OrderNotificationAccesor : IReflectorMetadata
	{
		public const int UserId = 1;
		public const int OrderId = 2;
		public const int Msg = 3;
		private readonly int[] _props = new []
		{
			UserId,OrderId,Msg
		};
		
		public int[] GetPropertyCodes() => _props;
		
		public string GetPropertyName(int propertyCode)
		{
		    return propertyCode switch
		    {
		       UserId => "UserId",
		       OrderId => "OrderId",
		       Msg => "Msg",
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public int GetPropertyCode(string propertyName)
		{
		    return propertyName switch
		    {
		        "UserId" => UserId,
		        "OrderId" => OrderId,
		        "Msg" => Msg,
		
		        _ => -1,
		    };
		}
		
		public Type GetPropertyType(int propertyCode)
		{
		    return propertyCode switch
		    {
		        UserId => typeof(string),
		        OrderId => typeof(string),
		        Msg => typeof(string),
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		       
		public object GetValue(object target, int propertyCode)
		{
		    OrderNotification obj = (OrderNotification)target;
		    return propertyCode switch
		    {
		        UserId => obj.UserId,
		        OrderId => obj.OrderId,
		        Msg => obj.Msg,
		
		        _ => throw new InvalidOperationException("property code not supported"),
		    };
		}
		
		public void SetValue(object target, int propertyCode, object value)
		{
		    OrderNotification obj = (OrderNotification)target;
		    switch (propertyCode)
		    {
		        case UserId:  obj.UserId = (string)value;break;
		        case OrderId:  obj.OrderId = (string)value;break;
		        case Msg:  obj.Msg = (string)value;break;
		
		        default: throw new InvalidOperationException("property code not supported");
		    }
		}
	
	}

}
