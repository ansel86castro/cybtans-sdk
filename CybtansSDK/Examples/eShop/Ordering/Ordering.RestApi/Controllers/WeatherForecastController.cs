using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Clients;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ordering.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly ICatalogService _catalogService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var response = await _catalogService.GetProducts();
            return response.Items;
        }

        [HttpGet("products/{id}")]
        public async Task<Product> GetProduct(int id)
        {
            var response = await _catalogService.GetProduct(new GetProductRequest { Id = id });
            return response;
        }

        [HttpPost("products")]
        public async Task<Product> CreateProduct([FromBody]Product item)
        {
            return await _catalogService.CreateProduct(item);
        }

        [HttpDelete("products/{id}")]
        public async Task DeleteProduct(int id)
        {
            await _catalogService.DeleteProduct(new DeleteRequest { Id = id });
        }
    }
}
