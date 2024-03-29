
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
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.RestApi.Controllers
{
	[Route("api/CustomerEvent")]
	[ApiController]
	public partial class CustomerEventServiceController : ControllerBase
	{
		private readonly ICustomerEventService _service;
		private readonly ILogger<CustomerEventServiceController> _logger;
		private readonly global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor _interceptor;
		
		public CustomerEventServiceController(ICustomerEventService service,  ILogger<CustomerEventServiceController> logger, global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor interceptor = null)
		{
			_service = service;
			_logger = logger;
			_interceptor = interceptor;
		}
		
		/// <summary>
		/// Returns a collection of CustomerEventDto
		/// </summary>
		[System.ComponentModel.Description("Returns a collection of CustomerEventDto")]
		[Authorize]
		[HttpGet]
		public async Task<models::GetAllCustomerEventResponse> GetAll([FromQuery]models::GetAllRequest request)
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
		/// Returns one CustomerEventDto by Id
		/// </summary>
		[System.ComponentModel.Description("Returns one CustomerEventDto by Id")]
		[Authorize]
		[HttpGet("{id}")]
		public async Task<models::CustomerEventDto> Get(Guid id, [FromQuery]models::GetCustomerEventRequest request)
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
		/// Creates one CustomerEventDto
		/// </summary>
		[System.ComponentModel.Description("Creates one CustomerEventDto")]
		[Authorize]
		[HttpPost]
		public async Task<models::CustomerEventDto> Create([FromBody]models::CreateCustomerEventRequest request)
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
		/// Updates one CustomerEventDto by Id
		/// </summary>
		[System.ComponentModel.Description("Updates one CustomerEventDto by Id")]
		[Authorize]
		[HttpPut("{id}")]
		public async Task<models::CustomerEventDto> Update(Guid id, [FromBody]models::UpdateCustomerEventRequest request)
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
		/// Deletes one CustomerEventDto by Id
		/// </summary>
		[System.ComponentModel.Description("Deletes one CustomerEventDto by Id")]
		[Authorize]
		[HttpDelete("{id}")]
		public async Task Delete(Guid id, [FromQuery]models::DeleteCustomerEventRequest request)
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
