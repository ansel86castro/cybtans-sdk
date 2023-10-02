
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

using models = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.RestApi.Controllers
{
	[Route("api/SoftDeleteOrder")]
	[ApiController]
	public partial class SoftDeleteOrderServiceController : ControllerBase
	{
		private readonly ISoftDeleteOrderService _service;
		private readonly ILogger<SoftDeleteOrderServiceController> _logger;
		private readonly global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor _interceptor;
		
		public SoftDeleteOrderServiceController(ISoftDeleteOrderService service,  ILogger<SoftDeleteOrderServiceController> logger, global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor interceptor = null)
		{
			_service = service;
			_logger = logger;
			_interceptor = interceptor;
		}
		
		/// <summary>
		/// Returns a collection of SoftDeleteOrderDto
		/// </summary>
		[System.ComponentModel.Description("Returns a collection of SoftDeleteOrderDto")]
		[HttpGet]
		public async Task<models::GetAllSoftDeleteOrderResponse> GetAll([FromQuery]models::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.GetAll(request).ConfigureAwait(false);
			return result;
		}
		
		/// <summary>
		/// Returns one SoftDeleteOrderDto by Id
		/// </summary>
		[System.ComponentModel.Description("Returns one SoftDeleteOrderDto by Id")]
		[HttpGet("{id}")]
		public async Task<models::SoftDeleteOrderDto> Get(Guid id, [FromQuery]models::GetSoftDeleteOrderRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.Get(request).ConfigureAwait(false);
			return result;
		}
		
		/// <summary>
		/// Creates one SoftDeleteOrderDto
		/// </summary>
		[System.ComponentModel.Description("Creates one SoftDeleteOrderDto")]
		[HttpPost]
		public async Task<models::SoftDeleteOrderDto> Create([FromBody]models::CreateSoftDeleteOrderRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.Create(request).ConfigureAwait(false);
			return result;
		}
		
		/// <summary>
		/// Updates one SoftDeleteOrderDto by Id
		/// </summary>
		[System.ComponentModel.Description("Updates one SoftDeleteOrderDto by Id")]
		[HttpPut("{id}")]
		public async Task<models::SoftDeleteOrderDto> Update(Guid id, [FromBody]models::UpdateSoftDeleteOrderRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.Update(request).ConfigureAwait(false);
			return result;
		}
		
		/// <summary>
		/// Deletes one SoftDeleteOrderDto by Id
		/// </summary>
		[System.ComponentModel.Description("Deletes one SoftDeleteOrderDto by Id")]
		[HttpDelete("{id}")]
		public async Task Delete(Guid id, [FromQuery]models::DeleteSoftDeleteOrderRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			await _service.Delete(request).ConfigureAwait(false);
		}
	}

}
