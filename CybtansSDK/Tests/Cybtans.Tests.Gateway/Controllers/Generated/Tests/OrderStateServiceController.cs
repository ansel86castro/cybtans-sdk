
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
	[Route("api/OrderState")]
	[ApiController]
	public partial class OrderStateServiceController : ControllerBase
	{
		private readonly IOrderStateServiceClient _service;
		private readonly ILogger<OrderStateServiceController> _logger;
		
		public OrderStateServiceController(IOrderStateServiceClient service,  ILogger<OrderStateServiceController> logger)
		{
			_service = service;
			_logger = logger;
		}
		
		/// <summary>
		/// Returns a collection of OrderStateDto
		/// </summary>
		[System.ComponentModel.Description("Returns a collection of OrderStateDto")]
		[Authorize(Roles = "admin")]
		[HttpGet]
		public async Task<mds::GetAllOrderStateResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			return await _service.GetAll(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Returns one OrderStateDto by Id
		/// </summary>
		[System.ComponentModel.Description("Returns one OrderStateDto by Id")]
		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public async Task<mds::OrderStateDto> Get(int id, [FromQuery]mds::GetOrderStateRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			return await _service.Get(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Creates one OrderStateDto
		/// </summary>
		[System.ComponentModel.Description("Creates one OrderStateDto")]
		[Authorize(Roles = "admin")]
		[HttpPost]
		public async Task<mds::OrderStateDto> Create([FromBody]mds::CreateOrderStateRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Create), request);
			
			return await _service.Create(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Updates one OrderStateDto by Id
		/// </summary>
		[System.ComponentModel.Description("Updates one OrderStateDto by Id")]
		[Authorize(Roles = "admin")]
		[HttpPut("{id}")]
		public async Task<mds::OrderStateDto> Update(int id, [FromBody]mds::UpdateOrderStateRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Update), request);
			
			return await _service.Update(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Deletes one OrderStateDto by Id
		/// </summary>
		[System.ComponentModel.Description("Deletes one OrderStateDto by Id")]
		[Authorize(Roles = "admin")]
		[HttpDelete("{id}")]
		public async Task Delete(int id, [FromQuery]mds::DeleteOrderStateRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Delete), request);
			
			await _service.Delete(request).ConfigureAwait(false);
		}
	}

}
