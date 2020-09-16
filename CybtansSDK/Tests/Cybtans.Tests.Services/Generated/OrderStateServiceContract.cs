using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public partial interface IOrderStateService 
	{
		
		Task<GetAllOrderStateResponse> GetAll(GetAllRequest request);
		
		
		Task<OrderStateDto> Get(GetOrderStateRequest request);
		
		
		Task<OrderStateDto> Create(CreateOrderStateRequest request);
		
		
		Task<OrderStateDto> Update(UpdateOrderStateRequest request);
		
		
		Task Delete(DeleteOrderStateRequest request);
		
	}

}
