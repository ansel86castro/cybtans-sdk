
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

namespace Cybtans.Tests.Controllers
{
	[Route("api/SoftDeleteOrder")]
	[ApiController]
	public partial class SoftDeleteOrderServiceController : ControllerBase
	{
		private readonly ISoftDeleteOrderService _service;
		
		public SoftDeleteOrderServiceController(ISoftDeleteOrderService service)
		{
			_service = service;
		}
		
		[HttpGet]
		public Task<mds::GetAllSoftDeleteOrderResponse> GetAll([FromQuery]mds::GetAllRequest request)
		{
			return _service.GetAll(request);
		}
		
		[HttpGet("{id}")]
		public Task<mds::SoftDeleteOrderDto> Get(Guid id, [FromQuery]mds::GetSoftDeleteOrderRequest request)
		{
			request.Id = id;
			return _service.Get(request);
		}
		
		[HttpPost]
		public Task<mds::SoftDeleteOrderDto> Create([FromBody]mds::CreateSoftDeleteOrderRequest request)
		{
			return _service.Create(request);
		}
		
		[HttpPut("{id}")]
		public Task<mds::SoftDeleteOrderDto> Update(Guid id, [FromBody]mds::UpdateSoftDeleteOrderRequest request)
		{
			request.Id = id;
			return _service.Update(request);
		}
		
		[HttpDelete("{id}")]
		public Task Delete(Guid id, [FromQuery]mds::DeleteSoftDeleteOrderRequest request)
		{
			request.Id = id;
			return _service.Delete(request);
		}
	}

}
