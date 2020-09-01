using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface IAuthicationService
	{
		
		[Post("//login")]
		Task<LoginResponse> Login([Body]LoginRequest request);
	
	}

}
