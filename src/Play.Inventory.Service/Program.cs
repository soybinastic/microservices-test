using System;
using System.Net.Http;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

const string CORS_POLICY = "CORSPOLICY";
// Add services to the container.

// add mongodb and repository.
builder.Services.AddMongo()
    .AddMongoRepository<InventoryItem>("inventory_items")
    .AddMongoRepository<CatalogItem>("catalog_items");
    
// add mass transit with rabbitmq
builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

builder.Services.AddCors(opt => 
    {
        opt.AddPolicy(CORS_POLICY, config =>
            config.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
    });

// synchronous communication via REST and HTTP.

// var jitterer = new Random();
// builder.Services.AddHttpClient<CatalogClient>(client => 
//     {
//         client.BaseAddress = new Uri("http://localhost:5182");
//     })
//     .AddTransientHttpErrorPolicy(opt => opt.Or<TimeoutRejectedException>().WaitAndRetryAsync(
//         5,
//         retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
//         onRetry: (outcome, timespan, retryAttempt) => 
//         {
//             var serviceProvider = builder.Services.BuildServiceProvider();
//             serviceProvider.GetService<ILogger<CatalogClient>>()?
//                 .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
//         }
//     ))
//     .AddTransientHttpErrorPolicy(opt => opt.Or<TimeoutRejectedException>().CircuitBreakerAsync(
//         3,
//         TimeSpan.FromSeconds(15),
//         onBreak : (outcome, timespan) => 
//         {
//             var serviceProvider = builder.Services.BuildServiceProvider();
//             serviceProvider.GetService<ILogger<CatalogClient>>()?
//                 .LogWarning($"Opening the circuit for {timespan.TotalSeconds} seconds...");
//         },
//         onReset: () => 
//         {
//             var serviceProvider = builder.Services.BuildServiceProvider();
//             serviceProvider.GetService<ILogger<CatalogClient>>()?
//                 .LogWarning("Closing the circuit...");
//         }
//     ))
//     .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

builder.Services.AddControllers();
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

app.UseCors(CORS_POLICY);
app.UseAuthorization();

app.MapControllers();

app.Run();
