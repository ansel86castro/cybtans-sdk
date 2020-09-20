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
		
		[Post("/api/Order/stream")]
		Task<UploadStreamResponse> UploadStream([Body]System.IO.Stream request);
		
		[Get("/api/Order/download")]
		Task<DowndloadImageResponse> DownloadImage(DownloadImageRequest request = null);
		
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
