using Cybtans.Refit;
using Cybtans.Test.Domain;
using Refit;
using System;
using System.Threading.Tasks;

namespace Cybtans.Tests.Services
{
    [ApiClient]
    public interface ICustomerClient
    {
        [Get("/api/customer/{id}")]
        Task<Customer> GetCustomer(Guid id);
    }
    

}
