using System;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;

namespace Cybtans.Tests.Controllers
{
	[Route("api/SoftDeleteOrder")]
	[ApiController]
	public partial class SoftDeleteOrderServiceController : ControllerBase
	{
		private readonly ISoftDeleteOrderService _service;
		
		public SoftDeleteOrderServiceController(ISoftDeleteOrderService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<GetAllSoftDeleteOrderResponse> GetAll([FromQuery]GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<SoftDeleteOrderDto> Get(Guid id, [FromQuery]GetSoftDeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<SoftDeleteOrderDto> Create([FromBody]SoftDeleteOrderDto __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<SoftDeleteOrderDto> Update(Guid id, [FromBody]UpdateSoftDeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]DeleteSoftDeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
