
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
	[Route("api/Customer")]
	[ApiController]
	public partial class CustomerServiceController : ControllerBase
	{
		private readonly ICustomerServiceClient _service;
		private readonly ILogger<CustomerServiceController> _logger;
		
		public CustomerServiceController(ICustomerServiceClient service,  ILogger<CustomerServiceController> logger)
		{
			_service = service;
			_logger = logger;
		}
		
		/// <summary>
		/// Returns a collection of CustomerDto
		/// </summary>
		[System.ComponentModel.Description("Returns a collection of CustomerDto")]
		[Authorize(Policy = "AdminUser")]
		[HttpGet]
		public async Task<mds::GetAllCustomerResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			return await _service.GetAll(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Returns one CustomerDto by Id
		/// </summary>
		[System.ComponentModel.Description("Returns one CustomerDto by Id")]
		[Authorize(Policy = "AdminUser")]
		[HttpGet("{id}")]
		public async Task<mds::CustomerDto> Get(Guid id, [FromQuery]mds::GetCustomerRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			return await _service.Get(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Creates one CustomerDto
		/// </summary>
		[System.ComponentModel.Description("Creates one CustomerDto")]
		[Authorize(Policy = "AdminUser")]
		[HttpPost]
		public async Task<mds::CustomerDto> Create([FromBody]mds::CreateCustomerRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			return await _service.Create(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Updates one CustomerDto by Id
		/// </summary>
		[System.ComponentModel.Description("Updates one CustomerDto by Id")]
		[Authorize(Policy = "AdminUser")]
		[HttpPut("{id}")]
		public async Task<mds::CustomerDto> Update(Guid id, [FromBody]mds::UpdateCustomerRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			return await _service.Update(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Deletes one CustomerDto by Id
		/// </summary>
		[System.ComponentModel.Description("Deletes one CustomerDto by Id")]
		[Authorize(Policy = "AdminUser")]
		[HttpDelete("{id}")]
		public async Task Delete(Guid id, [FromQuery]mds::DeleteCustomerRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			await _service.Delete(request).ConfigureAwait(false);
		}
	}

}