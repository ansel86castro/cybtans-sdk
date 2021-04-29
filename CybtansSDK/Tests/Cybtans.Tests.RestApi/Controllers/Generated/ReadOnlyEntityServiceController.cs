
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

using mds = global::Cybtans.Tests.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.Controllers
{
	[Route("api/ReadOnlyEntity")]
	[ApiController]
	public partial class ReadOnlyEntityServiceController : ControllerBase
	{
		private readonly IReadOnlyEntityService _service;
		
		public ReadOnlyEntityServiceController(IReadOnlyEntityService service)
		{
			_service = service;
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet]
		public Task<mds::GetAllReadOnlyEntityResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			return _service.GetAll(request);
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public Task<mds::ReadOnlyEntityDto> Get(int id, [FromQuery]mds::GetReadOnlyEntityRequest request)
		{
			request.Id = id;
			return _service.Get(request);
		}
	}

}
