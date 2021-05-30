
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using Cybtans.Tests.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
		private readonly IOrderService _service;
		private readonly ILogger<OrderServiceController> _logger;
		private readonly global::Cybtans.AspNetCore.Interceptors.IActionInterceptor _interceptor;
		
		public OrderServiceController(IOrderService service,  ILogger<OrderServiceController> logger, global::Cybtans.AspNetCore.Interceptors.IActionInterceptor interceptor = null)
		{
			_service = service;
			_logger = logger;
			_interceptor = interceptor;
		}
		
		[AllowAnonymous]
		[HttpGet("foo")]
		public async Task Foo()
		{
			_logger.LogInformation("Executing {Action}", nameof(Foo));
			
			await _service.Foo().ConfigureAwait(false);
		}
		
		[HttpGet("baar")]
		public async Task Baar()
		{
			_logger.LogInformation("Executing {Action}", nameof(Baar));
			
			await _service.Baar().ConfigureAwait(false);
		}
		
		[HttpGet("test")]
		public async Task Test()
		{
			_logger.LogInformation("Executing {Action}", nameof(Test));
			
			await _service.Test().ConfigureAwait(false);
		}
		
		[HttpGet("arg")]
		public async Task Argument()
		{
			_logger.LogInformation("Executing {Action}", nameof(Argument));
			
			await _service.Argument().ConfigureAwait(false);
		}
		
		/// <summary>
		/// Upload an image to the server
		/// </summary>
		[System.ComponentModel.Description("Upload an image to the server")]
		[HttpPost("upload")]
		[DisableFormValueModelBinding]
		public async Task<mds::UploadImageResponse> UploadImage([ModelBinder(typeof(CybtansModelBinder))]mds::UploadImageRequest request)
		{
			_logger.LogInformation("Executing {Action}", nameof(UploadImage));
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(UploadImage)).ConfigureAwait(false);
			}
			
			return await _service.UploadImage(request).ConfigureAwait(false);
		}
		
		[HttpPost("{id}/upload")]
		[DisableFormValueModelBinding]
		public async Task<mds::UploadStreamResponse> UploadStreamById(string id, [ModelBinder(typeof(CybtansModelBinder))]mds::UploadStreamByIdRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action}", nameof(UploadStreamById));
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(UploadStreamById)).ConfigureAwait(false);
			}
			
			return await _service.UploadStreamById(request).ConfigureAwait(false);
		}
		
		[HttpPost("ByteStream")]
		[DisableFormValueModelBinding]
		public async Task<mds::UploadStreamResponse> UploadStream([ModelBinder(typeof(CybtansModelBinder))]System.IO.Stream request)
		{
			_logger.LogInformation("Executing {Action}", nameof(UploadStream));
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(UploadStream)).ConfigureAwait(false);
			}
			
			return await _service.UploadStream(request).ConfigureAwait(false);
		}
		
		[HttpGet("download")]
		public async Task<IActionResult> DownloadImage([FromQuery]mds::DownloadImageRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(DownloadImage), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(DownloadImage)).ConfigureAwait(false);
			}
			
			var result = await _service.DownloadImage(request).ConfigureAwait(false);
			
			if(Request.Headers.ContainsKey("Accept") && System.Net.Http.Headers.MediaTypeHeaderValue.TryParse(Request.Headers["Accept"], out var mimeType) && mimeType?.MediaType == "application/x-cybtans")
			{				
				return new ObjectResult(result);
			}
			return new FileStreamResult(result.Image, result.ContentType) { FileDownloadName = result.FileName };
		}
		
		[HttpGet("{param1}multipath/{param2}")]
		public async Task GetMultiPath(string param1, string param2, [FromQuery]mds::MultiPathRequest request)
		{
			request.Param1 = param1;
			request.Param2 = param2;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetMultiPath), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(GetMultiPath)).ConfigureAwait(false);
			}
			
			await _service.GetMultiPath(request).ConfigureAwait(false);
		}
		
		[HttpPost("{orderId}/notify/{userId}")]
		public async Task SendNotification(string orderId, string userId, [FromBody]mds::OrderNotification request)
		{
			request.OrderId = orderId;
			request.UserId = userId;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(SendNotification), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(SendNotification)).ConfigureAwait(false);
			}
			
			await _service.SendNotification(request).ConfigureAwait(false);
		}
		
		[HttpGet("names")]
		public async Task<mds::GetAllNamesResponse> GetAllNames()
		{
			_logger.LogInformation("Executing {Action}", nameof(GetAllNames));
			
			return await _service.GetAllNames().ConfigureAwait(false);
		}
		
		[HttpGet("names/{id}")]
		public async Task<mds::OrderNamesDto> GetOrderName(string id, [FromQuery]mds::GetOrderNameRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetOrderName), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(GetOrderName)).ConfigureAwait(false);
			}
			
			return await _service.GetOrderName(request).ConfigureAwait(false);
		}
		
		[HttpPost("names")]
		public async Task<mds::OrderNamesDto> CreateOrderName([FromBody]mds::CreateOrderNameRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(CreateOrderName), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(CreateOrderName)).ConfigureAwait(false);
			}
			
			return await _service.CreateOrderName(request).ConfigureAwait(false);
		}
		
		[HttpGet]
		public async Task<mds::GetAllOrderResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(GetAll)).ConfigureAwait(false);
			}
			
			return await _service.GetAll(request).ConfigureAwait(false);
		}
		
		[HttpGet("{id}")]
		public async Task<mds::OrderDto> Get(Guid id, [FromQuery]mds::GetOrderRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Get)).ConfigureAwait(false);
			}
			
			return await _service.Get(request).ConfigureAwait(false);
		}
		
		[HttpPost]
		public async Task<mds::OrderDto> Create([FromBody]mds::CreateOrderRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Create)).ConfigureAwait(false);
			}
			
			return await _service.Create(request).ConfigureAwait(false);
		}
		
		[HttpPut("{id}")]
		public async Task<mds::OrderDto> Update(Guid id, [FromBody]mds::UpdateOrderRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Update)).ConfigureAwait(false);
			}
			
			return await _service.Update(request).ConfigureAwait(false);
		}
		
		[HttpDelete("{id}")]
		public async Task Delete(Guid id, [FromQuery]mds::DeleteOrderRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Delete)).ConfigureAwait(false);
			}
			
			await _service.Delete(request).ConfigureAwait(false);
		}
	}

}
