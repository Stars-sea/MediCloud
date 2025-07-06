using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MediCloud.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IDateTimeProvider     dateTimeProvider,
    IOptions<JwtSettings> jwtSettings
) : IJwtTokenGenerator {

    public string GenerateToken(User user) {
        JwtSettings settings = jwtSettings.Value;

        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken token = new(
            settings.Issuer,
            settings.Audience,
            expires: dateTimeProvider.UtcNow.AddMinutes(settings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
