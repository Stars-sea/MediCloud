using MediCloud.Api.Common.Errors;
using MediCloud.Domain.Entities;
using MediCloud.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MediCloud.Api;

public static class DependencyInjection {

    public static IServiceCollection AddPresentation(this IServiceCollection services) {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, MediCloudProblemDetailsFactory>();
        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services) {
        services
            .AddIdentity<User, IdentityRole>(options => {
                    options.User.RequireUniqueEmail         = true;
                    options.Password.RequireDigit           = true;
                    options.Password.RequiredLength         = 8;
                    options.Password.RequireNonAlphanumeric = false;
                }
            )
            .AddEntityFrameworkStores<MediCloudDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
