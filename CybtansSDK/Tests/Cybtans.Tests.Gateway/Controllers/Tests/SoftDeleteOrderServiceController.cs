
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
	[Route("api/SoftDeleteOrder")]
	[ApiController]
	public partial class SoftDeleteOrderServiceController : ControllerBase
	{
		private readonly ISoftDeleteOrderServiceClient _service;
		
		public SoftDeleteOrderServiceController(ISoftDeleteOrderServiceClient service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<mds::GetAllSoftDeleteOrderResponse> GetAll([FromQuery]mds::GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[HttpGet("{id}")]
		public Task<mds::SoftDeleteOrderDto> Get(Guid id, [FromQuery]mds::GetSoftDeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
		
		[HttpPost]
		public Task<mds::SoftDeleteOrderDto> Create([FromBody]mds::CreateSoftDeleteOrderRequest __request)
		{
			return _service.Create(__request);
		}
		
		[HttpPut("{id}")]
		public Task<mds::SoftDeleteOrderDto> Update(Guid id, [FromBody]mds::UpdateSoftDeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Update(__request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]mds::DeleteSoftDeleteOrderRequest __request)
		{
			__request.Id = id;
			return _service.Delete(__request);
		}
	}

}
