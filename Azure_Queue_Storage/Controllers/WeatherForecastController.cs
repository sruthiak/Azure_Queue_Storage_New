using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Azure_Queue_Storage.Controllers
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
        private readonly QueueClient client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,QueueClient client)
        {
            _logger = logger;
            this.client = client;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task Post([FromBody]WeatherForecast weatherForecast)
        {
            // once data is available in this endpoint it need to have many long running processing. 
            //So we can push these message to queue, and then have a background process or Azure Functions etc to process it.

            //connectionsting of storage account
            //var connectionString = "DefaultEndpointsProtocol=https;AccountName=stgaccountsruthi;AccountKey=lPIByl/+F2TO833HzwMytTkmv3SF25QjK3D62JblrqSUj2D840EM/51A8aeO+umnhFKWAk7A0quG+AStW3Iwlw==;EndpointSuffix=core.windows.net";
            //var queueName = "weatherdata";
            //var client = new QueueClient(connectionString, queueName);
            var message = JsonSerializer.Serialize(weatherForecast);
            await client.SendMessageAsync(message);
        }
    }
}
