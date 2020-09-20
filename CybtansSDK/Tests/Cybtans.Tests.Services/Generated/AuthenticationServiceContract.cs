using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	/// <summary>
	/// Jwt Authentication Service
	/// </summary>
	public partial interface IAuthenticationService 
	{
		
		/// <summary>
		/// Generates an access token
		/// </summary>
		Task<LoginResponse> Login(LoginRequest request);
		
	}

}
