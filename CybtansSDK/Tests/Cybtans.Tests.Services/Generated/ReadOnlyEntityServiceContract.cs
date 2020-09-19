using System;
using System.Threading.Tasks;
using Cybtans.Tests.Models;
using System.Collections.Generic;

namespace Cybtans.Tests.Services
{
	public partial interface IReadOnlyEntityService 
	{
		
		Task<GetAllReadOnlyEntityResponse> GetAll(GetAllRequest request);
		
		
		Task<ReadOnlyEntityDto> Get(GetReadOnlyEntityRequest request);
		
	}

}
