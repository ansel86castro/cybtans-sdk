using System;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;
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
		public Task<GetAllReadOnlyEntityResponse> GetAll([FromQuery]GetAllRequest __request)
		{
			return _service.GetAll(__request);
		}
		
		[Authorize(Roles = "admin")]
		[HttpGet("{id}")]
		public Task<ReadOnlyEntityDto> Get(int id, [FromQuery]GetReadOnlyEntityRequest __request)
		{
			__request.Id = id;
			return _service.Get(__request);
		}
	}

}
