using System;
using Cybtans.Tests.Services;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cybtans.Tests.RestApi.Controllers
{
	[Route("api/CustomerEvent")]
	[ApiController]
	public partial class CustomerEventServiceController : ControllerBase
	{
		private readonly ICustomerEventService _service;
		
		public CustomerEventServiceController(ICustomerEventService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<GetAllCustomerEventResponse> GetAll([FromQuery]GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<CustomerEventDto> Get(Guid id, [FromQuery]GetCustomerEventRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<CustomerEventDto> Create([FromBody]CustomerEventDto __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<CustomerEventDto> Update(Guid id, [FromBody]UpdateCustomerEventRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]DeleteCustomerEventRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
