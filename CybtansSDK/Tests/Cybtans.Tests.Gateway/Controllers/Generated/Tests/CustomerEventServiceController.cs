
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
using Microsoft.Extensions.Logging;

using mds = global::Cybtans.Tests.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.Controllers
{
	[Route("api/CustomerEvent")]
	[ApiController]
	public partial class CustomerEventServiceController : ControllerBase
	{
		private readonly ICustomerEventServiceClient _service;
		private readonly ILogger<CustomerEventServiceController> _logger;
		
		public CustomerEventServiceController(ICustomerEventServiceClient service,  ILogger<CustomerEventServiceController> logger)
		{
			_service = service;
			_logger = logger;
		}
		
		/// <summary>
		/// Returns a collection of CustomerEventDto
		/// </summary>
		[System.ComponentModel.Description("Returns a collection of CustomerEventDto")]
		[Authorize]
		[HttpGet]
		public async Task<mds::GetAllCustomerEventResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			return await _service.GetAll(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Returns one CustomerEventDto by Id
		/// </summary>
		[System.ComponentModel.Description("Returns one CustomerEventDto by Id")]
		[Authorize]
		[HttpGet("{id}")]
		public async Task<mds::CustomerEventDto> Get(Guid id, [FromQuery]mds::GetCustomerEventRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			return await _service.Get(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Creates one CustomerEventDto
		/// </summary>
		[System.ComponentModel.Description("Creates one CustomerEventDto")]
		[Authorize]
		[HttpPost]
		public async Task<mds::CustomerEventDto> Create([FromBody]mds::CreateCustomerEventRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			return await _service.Create(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Updates one CustomerEventDto by Id
		/// </summary>
		[System.ComponentModel.Description("Updates one CustomerEventDto by Id")]
		[Authorize]
		[HttpPut("{id}")]
		public async Task<mds::CustomerEventDto> Update(Guid id, [FromBody]mds::UpdateCustomerEventRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			return await _service.Update(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Deletes one CustomerEventDto by Id
		/// </summary>
		[System.ComponentModel.Description("Deletes one CustomerEventDto by Id")]
		[Authorize]
		[HttpDelete("{id}")]
		public async Task Delete(Guid id, [FromQuery]mds::DeleteCustomerEventRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			await _service.Delete(request).ConfigureAwait(false);
		}
	}

}
