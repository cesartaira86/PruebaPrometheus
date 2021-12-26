using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prometheus;

namespace PruebaPrometheus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly Histogram LoginDuration = Metrics
            .CreateHistogram("myapp_login_duration_seconds", "Histogram of login call processing durations.");

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private static readonly Counter ProcessedJobCount = Metrics
            .CreateCounter("myapp_jobs_processed_total", "Number of processed jobs.");
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            ProcessedJobCount.Inc();
            using (LoginDuration.NewTimer())
            {
                var rng = new Random();
                
                Random random = new Random();
                int num = random.Next(200);  
                
                Thread.Sleep(num);
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
}
