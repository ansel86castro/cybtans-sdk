
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
	/// <summary>
	/// Order's Service
	/// </summary>
	public interface IOrderService 
	{
		
		/// <summary>
		/// Hellow; "Func"
		/// </summary>
		Task Foo();
		
		
		Task Baar();
		
		
		Task Test();
		
		
		Task Argument();
		
		
		/// <summary>
		/// Upload an image to the server
		/// </summary>
		Task<models::UploadImageResponse> UploadImage(models::UploadImageRequest request);
		
		
		Task<models::UploadStreamResponse> UploadStreamById(models::UploadStreamByIdRequest request);
		
		
		Task<models::UploadStreamResponse> UploadStream(System.IO.Stream request);
		
		
		Task<models::DowndloadImageResponse> DownloadImage(models::DownloadImageRequest request);
		
		
		Task GetMultiPath(models::MultiPathRequest request);
		
		
		Task SendNotification(models::OrderNotification request);
		
		
		Task<models::GetAllNamesResponse> GetAllNames();
		
		
		Task<models::OrderNamesDto> GetOrderName(models::GetOrderNameRequest request);
		
		
		Task<models::OrderNamesDto> CreateOrderName(models::CreateOrderNameRequest request);
		
		
		/// <summary>
		/// Returns a collection of OrderDto
		/// </summary>
		Task<models::GetAllOrderResponse> GetAll(models::GetAllRequest request);
		
		
		/// <summary>
		/// Returns one OrderDto by Id
		/// </summary>
		Task<models::OrderDto> Get(models::GetOrderRequest request);
		
		
		/// <summary>
		/// Creates one OrderDto
		/// </summary>
		Task<models::OrderDto> Create(models::CreateOrderRequest request);
		
		
		/// <summary>
		/// Updates one OrderDto by Id
		/// </summary>
		Task<models::OrderDto> Update(models::UpdateOrderRequest request);
		
		
		/// <summary>
		/// Deletes one OrderDto by Id
		/// </summary>
		Task Delete(models::DeleteOrderRequest request);
		
	}

}
