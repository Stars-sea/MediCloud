using System.Reflection;
using FluentValidation;
using Mediator;
using MediCloud.Application.Common.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Application;

public static class DependencyInjection {

    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddValidators();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services) {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessageValidatorBehaviour<,>));

        return services;
    }
}
