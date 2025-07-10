using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediCloud.Infrastructure.Authentication;

public class JwtTokenManager(
    ICacheService         cacheService,
    IDateTimeProvider     dateTimeProvider,
    IOptions<JwtSettings> jwtSettings
) : IJwtTokenManager {

    public Result<JwtGenerateResult> GenerateToken(User user) {
        JwtSettings settings = jwtSettings.Value;

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(IJwtTokenManager.SecurityStampClaim, user.SecurityStamp)
        ];

        DateTime expires = dateTimeProvider.UtcNow.AddMinutes(settings.ExpiryMinutes);
        JwtSecurityToken token = new(
            settings.Issuer,
            settings.Audience,
            expires: expires,
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtGenerateResult(
            new JwtSecurityTokenHandler().WriteToken(token),
            expires
        );
    }

    public async Task<Result> BanTokenAsync(string jti, CancellationToken cancellationToken = default) {
        await cacheService.SetAsync($"jti.blacklist.{jti}", "", cancellationToken);
        return Result.Ok;
    }

    public Task BanTokenAsync(string jti, DateTime unbanTime, CancellationToken cancellationToken = default) {
        throw new NotImplementedException();
    }

    public async Task<Result> UnbanTokenAsync(string jti, CancellationToken cancellationToken = default) {
        await cacheService.RemoveAsync($"jti.blacklist.{jti}", cancellationToken);
        return Result.Ok;
    }

    public async Task<bool> IsTokenBanned(string jti, CancellationToken cancellationToken = default) {
        return await cacheService.GetAsync<string>($"jti.blacklist.{jti}", cancellationToken) is not null;
    }

}
