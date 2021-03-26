using System;
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
	public interface IOrderService
	{
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/foo")]
		Task Foo();
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/baar")]
		Task Baar();
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/test")]
		Task Test();
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/arg")]
		Task Argument();
		
		/// <summary>
		/// Upload an image to the server
		/// </summary>
		[Headers("Authorization: Bearer")]
		[Post("/api/Order/upload")]
		Task<UploadImageResponse> UploadImage([Body]UploadImageRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/Order/{request.Id}/upload")]
		Task<UploadStreamResponse> UploadStreamById([Body]UploadStreamByIdRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/Order/stream")]
		Task<UploadStreamResponse> UploadStream([Body]System.IO.Stream request);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/download")]
		Task<DowndloadImageResponse> DownloadImage(DownloadImageRequest request = null);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/{request.Param1}multipath/{request.Param2}")]
		Task GetMultiPath(MultiPathRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/Order/{request.OrderId}/notify/{request.UserId}")]
		Task SendNotification([Body]OrderNotification request);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order")]
		Task<GetAllOrderResponse> GetAll(GetAllRequest request = null);
		
		[Headers("Authorization: Bearer")]
		[Get("/api/Order/{request.Id}")]
		Task<OrderDto> Get(GetOrderRequest request);
		
		[Headers("Authorization: Bearer")]
		[Post("/api/Order")]
		Task<OrderDto> Create([Body]CreateOrderRequest request);
		
		[Headers("Authorization: Bearer")]
		[Put("/api/Order/{request.Id}")]
		Task<OrderDto> Update([Body]UpdateOrderRequest request);
		
		[Headers("Authorization: Bearer")]
		[Delete("/api/Order/{request.Id}")]
		Task Delete(DeleteOrderRequest request);
	
	}

}
