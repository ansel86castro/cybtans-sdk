using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface IOrderStateService
	{
		
		[Headers("Authorization: Bearer")]
		[Get("/api/OrderState")]
		Task<GetAllOrderStateResponse> GetAll(GetAllRequest request = null);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/OrderState/{request.Id}")]
		Task<OrderStateDto> Get(GetOrderStateRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/OrderState")]
		Task<OrderStateDto> Create([Body(buffered: true)]OrderStateDto request);
		
		[Headers("Authorization: Bearer")]
		[Put("/api/OrderState/{request.Id}")]
		Task<OrderStateDto> Update([Body(buffered: true)]UpdateOrderStateRequest request);
		
		[Headers("Authorization: Bearer")]
		[Delete("/api/OrderState/{request.Id}")]
		Task Delete(DeleteOrderStateRequest request);
	
	}

}
