
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using Cybtans.Tests.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Controllers
{
	[Route("api/Customer")]
	[ApiController]
	public partial class CustomerServiceController : ControllerBase
	{
		private readonly ICustomerService _service;
		
		public CustomerServiceController(ICustomerService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<mds::GetAllCustomerResponse> GetAll([FromQuery]mds::GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<mds::CustomerDto> Get(Guid id, [FromQuery]mds::GetCustomerRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<mds::CustomerDto> Create([FromBody]mds::CreateCustomerRequest __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<mds::CustomerDto> Update(Guid id, [FromBody]mds::UpdateCustomerRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]mds::DeleteCustomerRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
