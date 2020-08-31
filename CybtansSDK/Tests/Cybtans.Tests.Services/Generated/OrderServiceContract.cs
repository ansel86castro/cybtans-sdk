using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public partial interface IOrderService 
	{
		
		Task Foo();
		
		
		Task Baar();
		
		
		Task Test();
		
		
		Task Argument();
		
		
		Task<UploadImageResponse> UploadImage(UploadImageRequest request);
		
		
		Task<UploadStreamResponse> UploadStreamById(UploadStreamByIdRequest request);
		
		
		Task<UploadStreamResponse> UploadStream(System.IO.Stream request);
		
		
		Task<GetAllOrderResponse> GetAll(GetAllRequest request);
		
		
		Task<OrderDto> Get(GetOrderRequest request);
		
		
		Task<OrderDto> Create(OrderDto request);
		
		
		Task<OrderDto> Update(UpdateOrderRequest request);
		
		
		Task Delete(DeleteOrderRequest request);
		
	}

}
