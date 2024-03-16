using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace WeatherDataAzureFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(name:"ProcessWeatherData")]
        public void Run([QueueTrigger("weatherdata", Connection = "weatherDataCS")] QueueMessage message)
        {
            if (message.MessageText.Contains("exception")) throw new Exception("exception in message");
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
