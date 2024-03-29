
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
using Microsoft.Extensions.Logging;

using models = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.RestApi.Controllers
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
		private readonly ILogger<AuthenticationServiceController> _logger;
		private readonly global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor _interceptor;
		
		public AuthenticationServiceController(IAuthenticationService service,  ILogger<AuthenticationServiceController> logger, global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor interceptor = null)
		{
			_service = service;
			_logger = logger;
			_interceptor = interceptor;
		}
		
		/// <summary>
		/// Generates an access token
		/// </summary>
		[System.ComponentModel.Description("Generates an access token")]
		[HttpPost("login")]
		public async Task<models::LoginResponse> Login([FromBody]models::LoginRequest request)
		{
			_logger.LogInformation("Executing {Action} {Message}", nameof(Login), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.Login(request).ConfigureAwait(false);
			return result;
		}
	}

}
