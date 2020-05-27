using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Services;
using Ordering.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ordering.RestApi.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrdersServiceController : ControllerBase
	{
		private readonly OrdersService _service;
		
		public OrdersServiceController(OrdersService service)
		{
			_service = service;
		}
		
		[HttpGet("user/{userId}")]
		public async Task<GetOrdersResponse> GetOrdersByUser(int userId, [FromQuery]GetOrderByUserRequest __request)
		{
			__request.UserId = userId;
			return await _service.GetOrdersByUser(__request);
		}
		
		[HttpGet("{id}")]
		public async Task<Order> GetOrder(Guid id, [FromQuery]GetOrderRequest __request)
		{
			__request.Id = id;
			return await _service.GetOrder(__request);
		}
		
		[Authorize(Roles = "admin, order.create")]
		[HttpPost]
		public async Task<Order> CreateOrder([FromBody]Order __request)
		{
			return await _service.CreateOrder(__request);
		}
	}

}
