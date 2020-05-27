using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Clients;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Ordering.Clients;
using Ordering.Models;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IOrdersService _ordersService;

        public IndexModel(ILogger<IndexModel> logger, 
            ICatalogService catalogService,
            IOrdersService ordersService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _ordersService = ordersService;
        }

        public async Task OnGetAsync()
        {
            var catalogResponse = await _catalogService.GetProducts(new GetProductListRequest { });
            Products = catalogResponse.Items;

            var ordersResponse = await _ordersService.GetOrdersByUser(new GetOrderByUserRequest { UserId = 1 });
            Orders = ordersResponse.Orders;
        }
        
        public List<Product> Products { get; private set; }

        public List<Order> Orders { get; private set; }

    }
}
