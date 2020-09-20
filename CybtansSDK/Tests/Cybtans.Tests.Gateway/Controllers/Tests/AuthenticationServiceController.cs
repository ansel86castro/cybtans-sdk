using System;
using Cybtans.Tests.Clients;
using Cybtans.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cybtans.AspNetCore;

namespace Cybtans.Tests.Controllers
{
	/// <summary>
	/// Jwt Authentication Service
	/// </summary>
	[System.ComponentModel.Description("Jwt Authentication Service")]
	[Route("api/auth")]
	[ApiController]
	public partial class AuthenticationServiceController : ControllerBase
	{
		private readonly IAuthenticationService _service;
		
		public AuthenticationServiceController(IAuthenticationService service)
		{
			_service = service;
		}
		
		/// <summary>
		/// Generates an access token
		/// </summary>
		[System.ComponentModel.Description("Generates an access token")]
		[HttpPost("login")]
		public Task<LoginResponse> Login([FromBody]LoginRequest __request)
		{
			return _service.Login(__request);
		}
	}

}
