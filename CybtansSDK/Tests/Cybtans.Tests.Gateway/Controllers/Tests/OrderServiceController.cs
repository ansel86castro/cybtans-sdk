
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using Cybtans.Tests.Clients;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;

using mds = global::Cybtans.Tests.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.Controllers
{
	/// <summary>
	/// Order's Service
	/// </summary>
	[System.ComponentModel.Description("Order's Service")]
	[Route("api/Order")]
	[ApiController]
	public partial class OrderServiceController : ControllerBase
	{
		private readonly IOrderServiceClient _service;
		
		public OrderServiceController(IOrderServiceClient service)
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
		public Task<mds::UploadImageResponse> UploadImage([ModelBinder(typeof(CybtansModelBinder))]mds::UploadImageRequest request)
		{
			return _service.UploadImage(request);
		}
		
		[HttpPost("{id}/upload")]
		[DisableFormValueModelBinding]
		public Task<mds::UploadStreamResponse> UploadStreamById(string id, [ModelBinder(typeof(CybtansModelBinder))]mds::UploadStreamByIdRequest request)
		{
			request.Id = id;
			return _service.UploadStreamById(request);
		}
		
		[HttpPost("ByteStream")]
		[DisableFormValueModelBinding]
		public Task<mds::UploadStreamResponse> UploadStream([ModelBinder(typeof(CybtansModelBinder))]System.IO.Stream request)
		{
			return _service.UploadStream(request);
		}
		
		[HttpGet("download")]
		public async Task<IActionResult> DownloadImage([FromQuery]mds::DownloadImageRequest request)
		{
			var result = await _service.DownloadImage(request);
			
			 if(Request.Headers.ContainsKey("Accept")
				&& System.Net.Http.Headers.MediaTypeHeaderValue.TryParse(Request.Headers["Accept"], out var mimeType) && mimeType?.MediaType == "application/x-cybtans")
			{				
				return new ObjectResult(result);
			}
			return new FileStreamResult(result.Image, result.ContentType) { FileDownloadName = result.FileName };
		}
		
		[HttpGet("{param1}multipath/{param2}")]
		public Task GetMultiPath(string param1, string param2, [FromQuery]mds::MultiPathRequest request)
		{
			request.Param1 = param1;
			request.Param2 = param2;
			return _service.GetMultiPath(request);
		}
		
		[HttpPost("{orderId}/notify/{userId}")]
		public Task SendNotification(string orderId, string userId, [FromBody]mds::OrderNotification request)
		{
			request.OrderId = orderId;
			request.UserId = userId;
			return _service.SendNotification(request);
		}
		
		[HttpGet]
		public Task<mds::GetAllOrderResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			return _service.GetAll(request);
		}
		
		[HttpGet("{id}")]
		public Task<mds::OrderDto> Get(Guid id, [FromQuery]mds::GetOrderRequest request)
		{
			request.Id = id;
			return _service.Get(request);
		}
		
		[HttpPost]
		public Task<mds::OrderDto> Create([FromBody]mds::CreateOrderRequest request)
		{
			return _service.Create(request);
		}
		
		[HttpPut("{id}")]
		public Task<mds::OrderDto> Update(Guid id, [FromBody]mds::UpdateOrderRequest request)
		{
			request.Id = id;
			return _service.Update(request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]mds::DeleteOrderRequest request)
		{
			request.Id = id;
			return _service.Delete(request);
		}
	}

}
