using Azure.Storage.Queues;
using System.Text.Json;

namespace Azure_Queue_Storage
{
    public class WeatherDataServiceBackgroundJob : BackgroundService
    {
        private readonly ILogger<WeatherDataServiceBackgroundJob> _logger;
        private readonly QueueClient client;

        public WeatherDataServiceBackgroundJob(ILogger<WeatherDataServiceBackgroundJob> logger,QueueClient client)
        {
            _logger=logger;
            this.client = client;
        }   

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                
                var queueMsg= await client.ReceiveMessageAsync();
                if (queueMsg.Value != null)
                {
                    var weatherData = JsonSerializer.Deserialize<WeatherForecast>(queueMsg.Value.Body);
                    _logger.LogInformation("New Message received : {weatherData}", weatherData);

                    //Application logic to be done is written here

                    //once read the msg delete them from queue. Otherwise Dequeue count increased.
                    //DeQueue count is no of times the msg is read

                    await client.DeleteMessageAsync(queueMsg.Value.MessageId, queueMsg.Value.PopReceipt);
                }

                //wait for some time before checking the queue again
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
