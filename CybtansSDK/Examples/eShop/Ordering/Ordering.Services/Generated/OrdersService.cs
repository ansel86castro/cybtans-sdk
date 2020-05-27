using System;
using System.Threading.Tasks;
using Ordering.Models;
using System.Collections.Generic;

namespace Ordering.Services
{
	public abstract partial class OrdersService 
	{
		
		public abstract Task<GetOrdersResponse> GetOrdersByUser(GetOrderByUserRequest request);
		
		
		public abstract Task<Order> GetOrder(GetOrderRequest request);
		
		
		public abstract Task<Order> CreateOrder(Order request);
		
	}

}
