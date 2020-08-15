using System;
using Cybtans.Tests.Services;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.RestApi.Controllers
{
	[Route("api/OrderState")]
	[ApiController]
	public class OrderStateServiceController : ControllerBase
	{
		private readonly IOrderStateService _service;
		
		public OrderStateServiceController(IOrderStateService service)
		{
			_service = service;
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet]
		public Task<GetAllOrderStateResponse> GetAll([FromQuery]GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public Task<OrderStateDto> Get(int id, [FromQuery]GetOrderStateRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[Authorize(Roles = "admin")]
		[HttpPost]
		public Task<OrderStateDto> Create([FromBody]OrderStateDto __request)
		{
			return _service.Create(__request);
		}
		
		[Authorize(Roles = "admin")]
		[HttpPut("{id}")]
		public Task<OrderStateDto> Update(int id, [FromBody]UpdateOrderStateRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[Authorize(Roles = "admin")]
		[HttpDelete("{id}")]
		public Task Delete(int id, [FromQuery]DeleteOrderStateRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
