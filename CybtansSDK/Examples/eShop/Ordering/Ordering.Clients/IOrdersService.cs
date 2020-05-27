using System;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Ordering.Models;

namespace Ordering.Clients
{
	public interface IOrdersService
	{
		
		[Get("/api/orders/user/{request.UserId}")]
		Task<GetOrdersResponse> GetOrdersByUser(GetOrderByUserRequest request);
		
		[Get("/api/orders/{request.Id}")]
		Task<Order> GetOrder(GetOrderRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/orders")]
		Task<Order> CreateOrder([Body(buffered: true)]Order request);
	
	}

}
