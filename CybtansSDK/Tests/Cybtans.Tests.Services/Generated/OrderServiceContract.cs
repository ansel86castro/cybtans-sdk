using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	/// <summary>
	/// Order's Service
	/// </summary>
	public partial interface IOrderService 
	{
		
		Task Foo();
		
		
		Task Baar();
		
		
		Task Test();
		
		
		Task Argument();
		
		
		/// <summary>
		/// Upload an image to the server
		/// </summary>
		Task<UploadImageResponse> UploadImage(UploadImageRequest request);
		
		
		Task<UploadStreamResponse> UploadStreamById(UploadStreamByIdRequest request);
		
		
		Task<UploadStreamResponse> UploadStream(System.IO.Stream request);
		
		
		Task<DowndloadImageResponse> DownloadImage(DownloadImageRequest request);
		
		
		Task<GetAllOrderResponse> GetAll(GetAllRequest request);
		
		
		Task<OrderDto> Get(GetOrderRequest request);
		
		
		Task<OrderDto> Create(CreateOrderRequest request);
		
		
		Task<OrderDto> Update(UpdateOrderRequest request);
		
		
		Task Delete(DeleteOrderRequest request);
		
	}

}
