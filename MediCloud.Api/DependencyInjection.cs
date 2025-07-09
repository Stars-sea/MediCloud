using MediCloud.Api.Common.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MediCloud.Api;

public static class DependencyInjection {

    public static IServiceCollection AddPresentation(this IServiceCollection services) {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, MediCloudProblemDetailsFactory>();
        return services;
    }

}
