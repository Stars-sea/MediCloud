using System.Text;
using MassTransit;
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

namespace MediCloud.Test;

public static class DependencyInjection {

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddPersistence()
                .AddCachingService()
                .AddAuth(configuration)
                .AddMassTransitTest();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services) {
        services.AddDbContext<MediCloudDbContext>(options => {
            options.UseInMemoryDatabase("MediCloud");
            options.EnableSensitiveDataLogging(false);
        });
        
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddCachingService(this IServiceCollection services) {
        services.AddDistributedMemoryCache();

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        JwtSettings jwtSettings = configuration.GetSection(JwtSettings.SectionKey).Get<JwtSettings>()!;
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddSingleton<IJwtTokenBlacklist, JwtTokenBlacklist>();

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

    private static IServiceCollection AddMassTransitTest(this IServiceCollection services) {
        services.AddMassTransitTestHarness(options => {
                options.UsingInMemory((context, cfg) => {
                        cfg.ConfigureEndpoints(context);
                        cfg.UseDelayedMessageScheduler();
                    }
                );
                options.AddDelayedMessageScheduler();
                
                options.AddConsumers(typeof(MediCloud.Application.DependencyInjection).Assembly);
            }
        );
        return services;
    }

}
