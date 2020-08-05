using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public interface IOrderService 
	{
		
		Task<GetAllOrderResponse> GetAll(GetAllRequest request);
		
		
		Task<OrderDto> Get(GetOrderRequest request);
		
		
		Task<OrderDto> Create(OrderDto request);
		
		
		Task<OrderDto> Update(UpdateOrderRequest request);
		
		
		Task Delete(DeleteOrderRequest request);
		
	}

}
