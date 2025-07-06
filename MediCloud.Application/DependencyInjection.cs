using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Application;

public static class DependencyInjection {

    public static IServiceCollection AddApplication(this IServiceCollection services) {
        // TODO: Use RabbitMQ
        services.AddMassTransit(configure => {
                configure.AddConsumers(Assembly.GetExecutingAssembly());

                configure.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            }
        );

        return services;
    }

}
