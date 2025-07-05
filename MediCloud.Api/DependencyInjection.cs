using MediCloud.Domain.Entities;
using MediCloud.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace MediCloud.Api;

public static class DependencyInjection {
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
