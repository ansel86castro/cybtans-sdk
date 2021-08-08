using Cybtans.Messaging;
using System;

namespace Service2.Models
{
    [ExchangeRoute("Service2", "WeatherForecast")]
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
