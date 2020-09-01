using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface IAuthenticationService
	{
		
		[Post("/api/auth/login")]
		Task<LoginResponse> Login([Body]LoginRequest request);
	
	}

}
