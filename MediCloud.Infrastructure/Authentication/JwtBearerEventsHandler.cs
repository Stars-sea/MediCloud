using System.IdentityModel.Tokens.Jwt;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.User;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace MediCloud.Infrastructure.Authentication;

public class JwtBearerEventsHandler : JwtBearerEvents {

    public async override Task TokenValidated(TokenValidatedContext context) {
        if (!await VerifyJtiAsync(context)) {
            context.Fail("Current token is banned");
            return;
        }
        
        if (await GetUserFromContextAsync(context) is not { } user) {
            context.Fail("Invalid user");
            return;
        }
        
        if (!VerifySecurityStamp(context, user))
            context.Fail("Security stamp mismatch");
    }
    
    private async static Task<bool> VerifyJtiAsync(TokenValidatedContext context) {
        string? jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (jti is null) return false;
        
        var tokenManager = context.HttpContext.RequestServices.GetRequiredService<IJwtTokenManager>();

        return !await tokenManager.IsTokenBanned(jti);
    }

    private static Task<User?> GetUserFromContextAsync(TokenValidatedContext context) {
        try {
            UserId userId = UserId.Factory.Create(
                Guid.Parse(context.Principal!.FindFirst(JwtRegisteredClaimNames.Sub)!.Value)
            );

            var userRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            return userRepo.FindByIdAsync(userId);
        }
        catch {
            return Task.FromResult<User?>(null);
        }
    }

    private static bool VerifySecurityStamp(TokenValidatedContext context, User user) {
        string? securityStamp = context.Principal?.FindFirst(IJwtTokenManager.SecurityStampClaim)?.Value;
        string  currentStamp  = user.SecurityStamp;

        return string.Equals(securityStamp, currentStamp);
    }
}
