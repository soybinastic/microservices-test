using System.Reflection;
using Microsoft.Extensions.Configuration;
using Play.Common.Settings;

namespace Play.Common.MassTransit;


public static class Extension
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddMassTransit(configure => 
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((ctx, configurator) =>
                    {
                        var rabbitMQSettings = configuration
                            .GetSection(nameof(RabbitMQSettings))
                            .Get<RabbitMQSettings>()!;

                        var serviceSettings = configuration.GetSection(nameof(ServiceSettings))
                            .Get<ServiceSettings>()!;
                        
                        configurator.Host(rabbitMQSettings.Host);
                        configurator.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(
                            serviceSettings.ServiceName,
                            false
                        ));

                        configurator.UseMessageRetry(retryConfig => 
                            {
                                retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                            });
                    });
            });

        return services;
    }
}