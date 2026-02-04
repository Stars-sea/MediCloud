using Mediator;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Mappers;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Handlers;

public class RefreshTokenCommandHandler(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IJwtTokenBlacklist jwtTokenBlacklist
) : ICommandHandler<RefreshTokenCommand, Result<AuthenticationResult>> {

    public async ValueTask<Result<AuthenticationResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken) {
        (string email, string jti, string expiresStamp) = command;

        if (await userRepository.FindByEmailAsync(email) is not { } user)
            return Errors.User.UserNotFound;

        DateTimeOffset expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiresStamp));

        Result<JwtGenerateResult> generateResult = jwtTokenGenerator.GenerateToken(user);
        if (!generateResult.IsSuccess) return generateResult.Errors;

        await jwtTokenBlacklist.BanTokenAsync(jti, expires, cancellationToken);

        (string token, DateTimeOffset newTokenExpires) = generateResult.Value!;
        return user.MapResult(token, newTokenExpires);
    }

}
