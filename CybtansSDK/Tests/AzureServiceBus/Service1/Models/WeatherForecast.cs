using Cybtans.Messaging;
using System;

namespace Service1.Models
{
    [ExchangeRoute("Service1", "WeatherForecast")]
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
