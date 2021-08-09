using Cybtans.Entities;
using Cybtans.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service1.Controllers
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
        private readonly IMessageQueue _messageQueue ;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMessageQueue messageQueue)
        {
            _logger = logger;
            _messageQueue  = messageQueue;
        }

        [HttpPost]
        public async Task Create([FromBody] Model1 model)
        {
            _logger.LogInformation("Publishing {Id}", model.Id);

            await _messageQueue.Publish( new EntityCreated<Model1>(model), topic: EntityCreated<Model1>.TOPIC);
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
    }
}
