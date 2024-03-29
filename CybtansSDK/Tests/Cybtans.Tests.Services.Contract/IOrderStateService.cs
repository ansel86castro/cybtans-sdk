
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System.Threading.Tasks;

using models = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
	public interface IOrderStateService 
	{
		
		/// <summary>
		/// Returns a collection of OrderStateDto
		/// </summary>
		Task<models::GetAllOrderStateResponse> GetAll(models::GetAllRequest request);
		
		
		/// <summary>
		/// Returns one OrderStateDto by Id
		/// </summary>
		Task<models::OrderStateDto> Get(models::GetOrderStateRequest request);
		
		
		/// <summary>
		/// Creates one OrderStateDto
		/// </summary>
		Task<models::OrderStateDto> Create(models::CreateOrderStateRequest request);
		
		
		/// <summary>
		/// Updates one OrderStateDto by Id
		/// </summary>
		Task<models::OrderStateDto> Update(models::UpdateOrderStateRequest request);
		
		
		/// <summary>
		/// Deletes one OrderStateDto by Id
		/// </summary>
		Task Delete(models::DeleteOrderStateRequest request);
		
	}

}
