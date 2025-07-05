using System.Text;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Infrastructure.Authentication;
using MediCloud.Infrastructure.Persistence;
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
        services.AddDbContext<MediCloudDbContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("MEDI_CLOUD")!)
        );

        services.AddAuth(configuration);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        JwtSettings jwtSettings = configuration.GetSection(JwtSettings.SectionKey).Get<JwtSettings>()!;
        services.AddSingleton(Options.Create(jwtSettings));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = jwtSettings.Issuer,
                    ValidAudience            = jwtSettings.Audience,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                });
        return services;
    }
}
