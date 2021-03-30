
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   For support write to ansel@cybtans.com    
// </auto-generated>
//******************************************************

using System;
using Cybtans.Tests.Clients;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Controllers
{
	[Route("api/CustomerEvent")]
	[ApiController]
	public partial class CustomerEventServiceController : ControllerBase
	{
		private readonly ICustomerEventServiceClient _service;
		
		public CustomerEventServiceController(ICustomerEventServiceClient service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<mds::GetAllCustomerEventResponse> GetAll([FromQuery]mds::GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<mds::CustomerEventDto> Get(Guid id, [FromQuery]mds::GetCustomerEventRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<mds::CustomerEventDto> Create([FromBody]mds::CreateCustomerEventRequest __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<mds::CustomerEventDto> Update(Guid id, [FromBody]mds::UpdateCustomerEventRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]mds::DeleteCustomerEventRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
