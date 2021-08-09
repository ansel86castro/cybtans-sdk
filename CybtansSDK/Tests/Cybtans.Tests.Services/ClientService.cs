using Cybtans.Services;
using Cybtans.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cybtans.Tests.Services
{
    [RegisterDependency(typeof(IClientService))]
    public class ClientService : IClientService
    {
        List<ClientDto> _clients  = new List<ClientDto>
        {
            new ClientDto
           {
                ClientTypeId =1,
                CreatedAt= DateTime.UtcNow,
                Id = Guid.Parse("D6E29710-B68F-4D2D-9471-273DECF9C4B7"),
                Name = "Client 1",
                CreatorId = 1,
                ClientStatusId  = 1
            }
        };

        public async Task<ClientDto> GetClient(ClientRequest request)
        {
            return _clients .FirstOrDefault(x => x.Id == request.Id);
        }

        public async Task<ClientDto> GetClient2(ClientRequest request)
        {
            return _clients.FirstOrDefault(x => x.Id == request.Id);
        }

        public async Task<ClientDto> GetClient3(ClientRequest request)
        {
            return _clients.FirstOrDefault(x => x.Id == request.Id);
        }
    }
}
