using Cybtans.Services;
using Cybtans.Test.Domain;
using System;
using System.Threading.Tasks;

namespace Cybtans.Tests.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(Guid id);
    }

    [RegisterDependency(typeof(ICustomerService))]
    public class CustomerService : ICustomerService
    {
        public async Task<Customer> GetCustomer(Guid id)
        {
            return new Customer();
        }
    }
    

}
