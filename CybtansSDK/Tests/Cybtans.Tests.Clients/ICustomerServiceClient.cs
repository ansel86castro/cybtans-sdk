
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   For support write to ansel@cybtans.com    
// </auto-generated>
//******************************************************

using System;
using Refit;
using Cybtans.Refit;
using System.Net.Http;
using System.Threading.Tasks;
using Cybtans.Tests.Models;

namespace Cybtans.Tests.Clients
{
	[ApiClient]
	public interface ICustomerServiceClient
	{
		
		[Get("/api/Customer")]
		Task<GetAllCustomerResponse> GetAll(GetAllRequest request = null);
		
		[Get("/api/Customer/{request.Id}")]
		Task<CustomerDto> Get(GetCustomerRequest request);
		
		[Post("/api/Customer")]
		Task<CustomerDto> Create([Body]CreateCustomerRequest request);
		
		[Put("/api/Customer/{request.Id}")]
		Task<CustomerDto> Update([Body]UpdateCustomerRequest request);
		
		[Delete("/api/Customer/{request.Id}")]
		Task Delete(DeleteCustomerRequest request);
	
	}

}
