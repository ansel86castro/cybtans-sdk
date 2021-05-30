
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

using mds = global::Cybtans.Tests.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.Controllers
{
	[Route("api/OrderState")]
	[ApiController]
	public partial class OrderStateServiceController : ControllerBase
	{
		private readonly IOrderStateService _service;
		private readonly ILogger<OrderStateServiceController> _logger;
		private readonly global::Cybtans.AspNetCore.Interceptors.IActionInterceptor _interceptor;
		
		public OrderStateServiceController(IOrderStateService service,  ILogger<OrderStateServiceController> logger, global::Cybtans.AspNetCore.Interceptors.IActionInterceptor interceptor = null)
		{
			_service = service;
			_logger = logger;
			_interceptor = interceptor;
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet]
		public async Task<mds::GetAllOrderStateResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(GetAll)).ConfigureAwait(false);
			}
			
			return await _service.GetAll(request).ConfigureAwait(false);
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public async Task<mds::OrderStateDto> Get(int id, [FromQuery]mds::GetOrderStateRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Get)).ConfigureAwait(false);
			}
			
			return await _service.Get(request).ConfigureAwait(false);
		}
		
		[Authorize(Roles = "admin")]
		[HttpPost]
		public async Task<mds::OrderStateDto> Create([FromBody]mds::CreateOrderStateRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Create)).ConfigureAwait(false);
			}
			
			return await _service.Create(request).ConfigureAwait(false);
		}
		
		[Authorize(Roles = "admin")]
		[HttpPut("{id}")]
		public async Task<mds::OrderStateDto> Update(int id, [FromBody]mds::UpdateOrderStateRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request, nameof(Update)).ConfigureAwait(false);
			}
			
			return await _service.Update(request).ConfigureAwait(false);
		}
		
		[Authorize(Roles = "admin")]
		[HttpDelete("{id}")]
		public async Task Delete(int id, [FromQuery]mds::DeleteOrderStateRequest request)
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
