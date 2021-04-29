
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
	/// Order's Service
	/// </summary>
	[ApiClient]
	public interface IOrderServiceClient
	{
		
		[Get("/api/Order/foo")]
		Task Foo();
		
		[Get("/api/Order/baar")]
		Task Baar();
		
		[Get("/api/Order/test")]
		Task Test();
		
		[Get("/api/Order/arg")]
		Task Argument();
		
		/// <summary>
		/// Upload an image to the server
		/// </summary>
		[Post("/api/Order/upload")]
		Task<UploadImageResponse> UploadImage([Body]UploadImageRequest request);
		
		[Post("/api/Order/{request.Id}/upload")]
		Task<UploadStreamResponse> UploadStreamById([Body]UploadStreamByIdRequest request);
		
		[Post("/api/Order/ByteStream")]
		Task<UploadStreamResponse> UploadStream([Body]System.IO.Stream request);
		
		[Get("/api/Order/download")]
		Task<DowndloadImageResponse> DownloadImage(DownloadImageRequest request = null);
		
		[Get("/api/Order/{request.Param1}multipath/{request.Param2}")]
		Task GetMultiPath(MultiPathRequest request);
		
		[Post("/api/Order/{request.OrderId}/notify/{request.UserId}")]
		Task SendNotification([Body]OrderNotification request);
		
		[Get("/api/Order")]
		Task<GetAllOrderResponse> GetAll(GetAllRequest request = null);
		
		[Get("/api/Order/{request.Id}")]
		Task<OrderDto> Get(GetOrderRequest request);
		
		[Post("/api/Order")]
		Task<OrderDto> Create([Body]CreateOrderRequest request);
		
		[Put("/api/Order/{request.Id}")]
		Task<OrderDto> Update([Body]UpdateOrderRequest request);
		
		[Delete("/api/Order/{request.Id}")]
		Task Delete(DeleteOrderRequest request);
	
	}

}
