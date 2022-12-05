using Microsoft.Extensions.Configuration;
using Play.Common.Models;
using Play.Common.Settings;

namespace Play.Common.Repositories;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
            
        services.AddSingleton(sp => 
            {
                var configuration = sp.GetService<IConfiguration>()!;
                var mongoSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;

                var mongoClient = new MongoClient(mongoSettings?.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });
        
       return services;
    }

    public static IServiceCollection AddMongoRepository<T>(
        this IServiceCollection services, 
        string collection) 
        where T : IEntity
    {
        services.AddScoped<MongoDBRepository<T>>(serviceProvider => 
            {
                var database = serviceProvider.GetService<IMongoDatabase>()!;
                return new MongoRepository<T>(database, collection);
            });

        return services;
    }
}