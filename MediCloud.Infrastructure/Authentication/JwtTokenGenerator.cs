using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediCloud.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IDateTimeProvider     dateTimeProvider,
    IOptions<JwtSettings> jwtSettings
) : IJwtTokenGenerator {
    public string GenerateToken(string username, string userId, string email) {
        JwtSettings settings = jwtSettings.Value;

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Name, username),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken token = new(
            issuer: settings.Issuer,
            audience: settings.Audience,
            expires: dateTimeProvider.UtcNow.AddMinutes(settings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
