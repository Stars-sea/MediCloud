using System.Text;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Infrastructure.Authentication;
using MediCloud.Infrastructure.Persistence;
using MediCloud.Infrastructure.Persistence.Repositories;
using MediCloud.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediCloud.Infrastructure;

public static class DependencyInjection {

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddPersistence(configuration);
        services.AddAuth(configuration);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IPasswordValidator, PasswordValidator>();

        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<MediCloudDbContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("MEDI_CLOUD")!)
        );

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        JwtSettings jwtSettings = configuration.GetSection(JwtSettings.SectionKey).Get<JwtSettings>()!;
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenManager, JwtTokenManager>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                        options.TokenValidationParameters = new TokenValidationParameters {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                        };
                        options.MapInboundClaims = false;

                        options.Events = new JwtBearerEventsHandler();
                    }
                );
        return services;
    }

}
