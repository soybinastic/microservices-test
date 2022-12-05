
using Play.Common.Settings;

const string CORS_POLICY = "CORSPOLICY";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceSettings = builder.Configuration
    .GetSection(nameof(ServiceSettings))
    .Get<ServiceSettings>()!;
// add mongodb and repository.
builder.Services.AddMongo()
    .AddMongoRepository<Item>("items");

// add masstransit with rabbitmq to the service.
builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

builder.Services.AddCors(opt => 
    {
        opt.AddPolicy(CORS_POLICY, config => 
            config.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
    });
// builder.Services.AddMassTransitHostedService();

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
