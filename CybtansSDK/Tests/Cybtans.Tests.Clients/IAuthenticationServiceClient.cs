
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	/// <summary>
	/// Jwt Authentication Service
	/// </summary>
	[ApiClient]
	public interface IAuthenticationServiceClient
	{
		
		/// <summary>
		/// Generates an access token
		/// </summary>
		[Post("/api/auth/login")]
		Task<LoginResponse> Login([Body]LoginRequest request);
	
	}

}
