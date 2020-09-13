using System;
using Cybtans.Tests.Services;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;

namespace Cybtans.Tests.Controllers
{
	[Route("api/Customer")]
	[ApiController]
	public partial class CustomerServiceController : ControllerBase
	{
		private readonly ICustomerService _service;
		
		public CustomerServiceController(ICustomerService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<GetAllCustomerResponse> GetAll([FromQuery]GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<CustomerDto> Get(Guid id, [FromQuery]GetCustomerRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<CustomerDto> Create([FromBody]CustomerDto __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<CustomerDto> Update(Guid id, [FromBody]UpdateCustomerRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]DeleteCustomerRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
