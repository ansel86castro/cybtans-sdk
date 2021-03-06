
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
	[Route("api/ReadOnlyEntity")]
	[ApiController]
	public partial class ReadOnlyEntityServiceController : ControllerBase
	{
		private readonly IReadOnlyEntityServiceClient _service;
		private readonly ILogger<ReadOnlyEntityServiceController> _logger;
		
		public ReadOnlyEntityServiceController(IReadOnlyEntityServiceClient service,  ILogger<ReadOnlyEntityServiceController> logger)
		{
			_service = service;
			_logger = logger;
		}
		
		/// <summary>
		/// Returns a collection of ReadOnlyEntityDto
		/// </summary>
		[System.ComponentModel.Description("Returns a collection of ReadOnlyEntityDto")]
		[Authorize(Roles = "admin")]
		[HttpGet]
		public async Task<mds::GetAllReadOnlyEntityResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetAll), request);
			
			return await _service.GetAll(request).ConfigureAwait(false);
		}
		
		/// <summary>
		/// Returns one ReadOnlyEntityDto by Id
		/// </summary>
		[System.ComponentModel.Description("Returns one ReadOnlyEntityDto by Id")]
		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public async Task<mds::ReadOnlyEntityDto> Get(int id, [FromQuery]mds::GetReadOnlyEntityRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(Get), request);
			
			return await _service.Get(request).ConfigureAwait(false);
		}
	}

}
