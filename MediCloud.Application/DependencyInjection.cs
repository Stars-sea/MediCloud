using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Application;

public static class DependencyInjection {

    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddMediator(config =>
            config.AddConsumers(Assembly.GetExecutingAssembly())
        );

        return services;
    }

}
