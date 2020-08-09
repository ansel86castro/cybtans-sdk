using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface ICustomerEventService
	{
		
		[Get("/api/CustomerEvent")]
		Task<GetAllCustomerEventResponse> GetAll(GetAllRequest request = null);
		
		[Get("/api/CustomerEvent/{request.Id}")]
		Task<CustomerEventDto> Get(GetCustomerEventRequest request);
		
		[Post("/api/CustomerEvent")]
		Task<CustomerEventDto> Create([Body(buffered: true)]CustomerEventDto request);
		
		[Put("/api/CustomerEvent/{request.Id}")]
		Task<CustomerEventDto> Update([Body(buffered: true)]UpdateCustomerEventRequest request);
		
		[Delete("/api/CustomerEvent/{request.Id}")]
		Task Delete(DeleteCustomerEventRequest request);
	
	}

}
