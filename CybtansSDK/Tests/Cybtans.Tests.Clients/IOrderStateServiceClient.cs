
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
	public interface IOrderStateServiceClient
	{
		
		[Headers("Authorization: Bearer")]
		[Get("/api/OrderState")]
		Task<GetAllOrderStateResponse> GetAll(GetAllRequest request = null);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/OrderState/{request.Id}")]
		Task<OrderStateDto> Get(GetOrderStateRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/OrderState")]
		Task<OrderStateDto> Create([Body]CreateOrderStateRequest request);
		
		[Headers("Authorization: Bearer")]
		[Put("/api/OrderState/{request.Id}")]
		Task<OrderStateDto> Update([Body]UpdateOrderStateRequest request);
		
		[Headers("Authorization: Bearer")]
		[Delete("/api/OrderState/{request.Id}")]
		Task Delete(DeleteOrderStateRequest request);
	
	}

}
