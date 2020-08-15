using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface IOrderService
	{
		
		[Get("/api/Order/foo")]
		Task Foo();
		
		[Get("/api/Order/baar")]
		Task Baar();
		
		[Get("/api/Order/test")]
		Task Test();
		
		[Get("/api/Order")]
		Task<GetAllOrderResponse> GetAll(GetAllRequest request = null);
		
		[Get("/api/Order/{request.Id}")]
		Task<OrderDto> Get(GetOrderRequest request);
		
		[Post("/api/Order")]
		Task<OrderDto> Create([Body(buffered: true)]OrderDto request);
		
		[Put("/api/Order/{request.Id}")]
		Task<OrderDto> Update([Body(buffered: true)]UpdateOrderRequest request);
		
		[Delete("/api/Order/{request.Id}")]
		Task Delete(DeleteOrderRequest request);
	
	}

}
