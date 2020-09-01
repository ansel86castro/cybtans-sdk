using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public partial interface IAuthenticationService 
	{
		
		Task<LoginResponse> Login(LoginRequest request);
		
	}

}
