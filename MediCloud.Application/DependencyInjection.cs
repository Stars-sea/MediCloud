using System.Reflection;
using FluentValidation;
using MassTransit;
using MediCloud.Application.Common.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Application;

public static class DependencyInjection {

    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediator(x => {
                x.ConfigureMediator((context, cfg) => cfg.UseConsumeFilter(typeof(ValidationConsumeFilter<>), context));
                x.AddConsumers(Assembly.GetExecutingAssembly());
            }
        );

        return services;
    }

}
