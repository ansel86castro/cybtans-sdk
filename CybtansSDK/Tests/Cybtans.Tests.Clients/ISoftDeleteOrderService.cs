using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface ISoftDeleteOrderService
	{
		
		[Get("/api/SoftDeleteOrder")]
		Task<GetAllSoftDeleteOrderResponse> GetAll(GetAllRequest request = null);
		
		[Get("/api/SoftDeleteOrder/{request.Id}")]
		Task<SoftDeleteOrderDto> Get(GetSoftDeleteOrderRequest request);
		
		[Post("/api/SoftDeleteOrder")]
		Task<SoftDeleteOrderDto> Create([Body(buffered: true)]SoftDeleteOrderDto request);
		
		[Put("/api/SoftDeleteOrder/{request.Id}")]
		Task<SoftDeleteOrderDto> Update([Body(buffered: true)]UpdateSoftDeleteOrderRequest request);
		
		[Delete("/api/SoftDeleteOrder/{request.Id}")]
		Task Delete(DeleteSoftDeleteOrderRequest request);
	
	}

}
