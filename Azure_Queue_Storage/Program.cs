using Azure.Storage.Queues;
using Azure_Queue_Storage;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// add Background service to our api. Commenting because of using Azure Function
//builder.Services.AddHostedService<WeatherDataServiceBackgroundJob>();

// add queue client
builder.Services.AddAzureClients(builder =>
{
    builder.AddClient<QueueClient, QueueClientOptions>((options,_,_) =>
    {
        //queue trigger of Azure Functions requires the message to be base64 encoded
        options.MessageEncoding = QueueMessageEncoding.Base64;
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=stgaccountsruthi;AccountKey=lPIByl/+F2TO833HzwMytTkmv3SF25QjK3D62JblrqSUj2D840EM/51A8aeO+umnhFKWAk7A0quG+AStW3Iwlw==;EndpointSuffix=core.windows.net";
        var queueName = "weatherdata";
        return new QueueClient(connectionString, queueName,options);
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
