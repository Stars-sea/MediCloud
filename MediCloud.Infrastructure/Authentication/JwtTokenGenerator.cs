using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Settings;
using MediCloud.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediCloud.Infrastructure.Authentication;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator {

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
            new(IJwtTokenBlacklist.SecurityStampClaim, user.SecurityStamp)
        ];

        DateTimeOffset expires = DateTimeOffset.UtcNow.AddMinutes(settings.ExpiryMinutes);
        JwtSecurityToken token = new(
            settings.Issuer,
            settings.Audience,
            expires: expires.UtcDateTime,
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtGenerateResult(
            new JwtSecurityTokenHandler().WriteToken(token),
            expires
        );
    }

}
