
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
using Microsoft.AspNetCore.Authorization;

namespace Cybtans.Tests.RestApi.Controllers
{
	[Authorize]
	[Route("api/clients")]
	[ApiController]
	public partial class ClientServiceController : ControllerBase
	{
		private readonly IClientService _service;
		private readonly ILogger<ClientServiceController> _logger;
		private readonly global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor _interceptor;
		private readonly global::Microsoft.AspNetCore.Authorization.IAuthorizationService _authorizationService;
		
		public ClientServiceController(IClientService service,  ILogger<ClientServiceController> logger, global::Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService, global::Cybtans.AspNetCore.Interceptors.IMessageInterceptor interceptor = null)
		{
			_service = service;
			_logger = logger;
			_interceptor = interceptor;
			_authorizationService = authorizationService;
		}
		
		[HttpGet("{id}")]
		public async Task<models::ClientDto> GetClient(Guid id, [FromQuery]models::ClientRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetClient), request);
			
			var authRequestResult = await _authorizationService.AuthorizeAsync(User, request, "ClientPolicy").ConfigureAwait(false);
			if (!authRequestResult.Succeeded)
			{
			    throw new UnauthorizedAccessException($"Request Authorization Failed: { string.Join(", ", authRequestResult.Failure.FailedRequirements) }");
			}
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.GetClient(request).ConfigureAwait(false);
			if (result != null)
			{
			    var authResult = await _authorizationService.AuthorizeAsync(User, result, "ClientCreator").ConfigureAwait(false);
			    if (!authResult.Succeeded)
			    {
			        throw new UnauthorizedAccessException($"Result Authorization Failed: { string.Join(", ", authResult.Failure.FailedRequirements) }");
			    }
			}
			
			return result;
		}
		
		[HttpGet("client2/{id}")]
		public async Task<models::ClientDto> GetClient2(Guid id, [FromQuery]models::ClientRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetClient2), request);
			
			var authRequestResult = await _authorizationService.AuthorizeAsync(User, request, "ClientPolicy").ConfigureAwait(false);
			if (!authRequestResult.Succeeded)
			{
			    throw new UnauthorizedAccessException($"Request Authorization Failed: { string.Join(", ", authRequestResult.Failure.FailedRequirements) }");
			}
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.GetClient2(request).ConfigureAwait(false);
			return result;
		}
		
		[HttpGet("client3/{id}")]
		public async Task<models::ClientDto> GetClient3(Guid id, [FromQuery]models::ClientRequest request)
		{
			request.Id = id;
			
			_logger.LogInformation("Executing {Action} {Message}", nameof(GetClient3), request);
			
			if(_interceptor != null )
			{
			    await _interceptor.Handle(request).ConfigureAwait(false);
			}
			
			var result = await _service.GetClient3(request).ConfigureAwait(false);
			if (result != null)
			{
			    var authResult = await _authorizationService.AuthorizeAsync(User, result, "ClientCreator").ConfigureAwait(false);
			    if (!authResult.Succeeded)
			    {
			        throw new UnauthorizedAccessException($"Result Authorization Failed: { string.Join(", ", authResult.Failure.FailedRequirements) }");
			    }
			}
			
			return result;
		}
	}

}
