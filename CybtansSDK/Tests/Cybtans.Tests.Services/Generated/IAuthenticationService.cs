
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System;
using System.Threading.Tasks;

using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
	/// <summary>
	/// Jwt Authentication Service
	/// </summary>
	public interface IAuthenticationService 
	{
		
		/// <summary>
		/// Generates an access token
		/// </summary>
		Task<mds::LoginResponse> Login(mds::LoginRequest request);
		
	}

}