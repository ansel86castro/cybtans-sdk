using System;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.Controllers
{
	/// <summary>
	/// Order's Service
	/// </summary>
	[System.ComponentModel.Description("Order's Service")]
	[Authorize]
	[Route("api/Order")]
	[ApiController]
	public partial class OrderServiceController : ControllerBase
	{
		private readonly IOrderService _service;
		
		public OrderServiceController(IOrderService service)
		{
			_service = service;
		}
		
		[AllowAnonymous]
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
		
		/// <summary>
		/// Upload an image to the server
		/// </summary>
		[System.ComponentModel.Description("Upload an image to the server")]
		[HttpPost("upload")]
		[DisableFormValueModelBinding]
		public Task<UploadImageResponse> UploadImage([ModelBinder(typeof(CybtansModelBinder))]UploadImageRequest __request)
		{
			return _service.UploadImage(__request);
		}
		
		[HttpPost("{id}/upload")]
		[DisableFormValueModelBinding]
		public Task<UploadStreamResponse> UploadStreamById(string id, [ModelBinder(typeof(CybtansModelBinder))]UploadStreamByIdRequest __request)
		{
			__request.Id = id;
			return _service.UploadStreamById(__request);
		}
		
		[HttpPost("stream")]
		[DisableFormValueModelBinding]
		public Task<UploadStreamResponse> UploadStream([ModelBinder(typeof(CybtansModelBinder))]System.IO.Stream __request)
		{
			return _service.UploadStream(__request);
		}
		
		[HttpGet("download")]
		public async Task<IActionResult> DownloadImage([FromQuery]DownloadImageRequest __request)
		{
			var result = await _service.DownloadImage(__request);
			
			 if(Request.Headers.ContainsKey("Accept")
				&& System.Net.Http.Headers.MediaTypeHeaderValue.TryParse(Request.Headers["Accept"], out var mimeType) && mimeType?.MediaType == "application/x-cybtans")
			{				
				return new ObjectResult(result);
			}
			return new FileStreamResult(result.Image, result.ContentType) { FileDownloadName = result.FileName };
		}
		
		[HttpGet("{param1}multipath/{param2}")]
		public Task GetMultiPath(string param1, string param2, [FromQuery]MultiPathRequest __request)
		{
			__request.Param1 = param1;
			__request.Param2 = param2;
			return _service.GetMultiPath(__request);
		}
		
		[HttpPost("{orderId}/notify/{userId}")]
		public Task SendNotification(string orderId, string userId, [FromBody]OrderNotification __request)
		{
			__request.OrderId = orderId;
			__request.UserId = userId;
			return _service.SendNotification(__request);
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
		public Task<OrderDto> Create([FromBody]CreateOrderRequest __request)
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
