using Cybtans.Tests.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Cybtans.Tests.RestApi.Filters
{
    public class OrdersFilter : IAsyncActionFilter
    {
        private readonly IOrderService _service ;

        public OrdersFilter(IOrderService service)
        {
            _service  = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {            
            await next().ConfigureAwait(false);
        }
    }
}
