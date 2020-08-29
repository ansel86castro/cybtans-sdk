using System;
using Cybtans.Tests.Services;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;

namespace Cybtans.Tests.RestApi.Controllers
{
	[Route("api/Order")]
	[ApiController]
	public partial class OrderServiceController : ControllerBase
	{
		private readonly IOrderService _service;
		
		public OrderServiceController(IOrderService service)
		{
			_service = service;
		}
		
		[HttpGet("foo")]
		public Task Foo()
		{
			return _service.Foo();
		}
		
		[HttpGet("baar")]
		public Task Baar()
		{
			return _service.Baar();
		}
		
		[HttpGet("test")]
		public Task Test()
		{
			return _service.Test();
		}
		
		[HttpGet("arg")]
		public Task Argument()
		{
			return _service.Argument();
		}
		
		[HttpPost("upload")]
		[DisableFormValueModelBinding]
		public Task<UploadImageResponse> UploadImage([ModelBinder(BinderType = typeof(CybtansModelBinder))]UploadImageRequest __request)
		{
			return _service.UploadImage(__request);
		}
		
		[HttpGet]
		public Task<GetAllOrderResponse> GetAll([FromQuery]GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<OrderDto> Get(Guid id, [FromQuery]GetOrderRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<OrderDto> Create([FromBody]OrderDto __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<OrderDto> Update(Guid id, [FromBody]UpdateOrderRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]DeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
