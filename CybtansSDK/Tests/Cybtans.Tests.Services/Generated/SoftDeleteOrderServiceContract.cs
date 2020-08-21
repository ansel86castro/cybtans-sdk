using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public interface ISoftDeleteOrderService 
	{
		
		Task<GetAllSoftDeleteOrderResponse> GetAll(GetAllRequest request);
		
		
		Task<SoftDeleteOrderDto> Get(GetSoftDeleteOrderRequest request);
		
		
		Task<SoftDeleteOrderDto> Create(SoftDeleteOrderDto request);
		
		
		Task<SoftDeleteOrderDto> Update(UpdateSoftDeleteOrderRequest request);
		
		
		Task Delete(DeleteSoftDeleteOrderRequest request);
		
	}

}
