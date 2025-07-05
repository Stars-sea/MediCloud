using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Infrastructure.Authentication;
using MediCloud.Infrastructure.Persistence;
using MediCloud.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<MediCloudDbContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("MEDI_CLOUD")!)
        );

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionKey));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
