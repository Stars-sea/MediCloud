using MediCloud.Api.Common.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MediCloud.Api;

public static class DependencyInjection {

    public static IServiceCollection AddPresentation(this IServiceCollection services) {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, MediCloudProblemDetailsFactory>();

        services.ConfigureMediator();
        
        return services;
    }

    private static IServiceCollection ConfigureMediator(this IServiceCollection services) {
        return services.AddMediator(options => {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });
    }

}
