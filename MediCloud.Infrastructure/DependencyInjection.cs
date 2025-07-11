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

namespace MediCloud.Infrastructure;

public static class DependencyInjection {

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddPersistence(configuration)
                .AddCachingService(configuration)
                .AddAuth(configuration)
                .AddMassTransit(configuration);

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<MediCloudDbContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("Database")!)
        );

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddCachingService(
        this IServiceCollection services,
        IConfiguration          configuration
    ) {
        services.AddStackExchangeRedisCache(options => {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName  = "MediCloud";
            }
        );

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

    private static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration) {
        services.AddMassTransit(options => {
                options.UsingRabbitMq((context, cfg) => {
                        cfg.Host(configuration.GetConnectionString("RabbitMQ"));
                        cfg.ConfigureEndpoints(context);
                        cfg.UseDelayedMessageScheduler();
                    }
                );
                options.AddDelayedMessageScheduler();

                options.AddConsumers(typeof(Application.DependencyInjection).Assembly);
            }
        );
        return services;
    }

}
