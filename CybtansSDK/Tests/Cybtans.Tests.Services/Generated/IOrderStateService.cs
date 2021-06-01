
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

using System.Threading.Tasks;

using mds = global::Cybtans.Tests.Models;

namespace Cybtans.Tests.Services
{
	public interface IOrderStateService 
	{
		
		/// <summary>
		/// Returns a collection of OrderStateDto
		/// </summary>
		Task<mds::GetAllOrderStateResponse> GetAll(mds::GetAllRequest request);
		
		
		/// <summary>
		/// Returns one OrderStateDto by Id
		/// </summary>
		Task<mds::OrderStateDto> Get(mds::GetOrderStateRequest request);
		
		
		/// <summary>
		/// Creates one OrderStateDto
		/// </summary>
		Task<mds::OrderStateDto> Create(mds::CreateOrderStateRequest request);
		
		
		/// <summary>
		/// Updates one OrderStateDto by Id
		/// </summary>
		Task<mds::OrderStateDto> Update(mds::UpdateOrderStateRequest request);
		
		
		/// <summary>
		/// Deletes one OrderStateDto by Id
		/// </summary>
		Task Delete(mds::DeleteOrderStateRequest request);
		
	}

}
