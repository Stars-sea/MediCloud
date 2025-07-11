using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Handlers;

public class RefreshTokenCommandHandler(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResult>> {

    public async Task<Result<AuthenticationResult>> Handle(
        RefreshTokenCommand                 request,
        ConsumeContext<RefreshTokenCommand> ctx
    ) {
        (string email, string jti, string expiresStamp) = request;

        if (await userRepository.FindByEmailAsync(email) is not { } user)
            return Errors.User.UserNotFound;

        DateTimeOffset expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiresStamp));

        Result<JwtGenerateResult> generateResult = jwtTokenGenerator.GenerateToken(user);
        if (!generateResult.IsSuccess) return generateResult.Errors;

        await ctx.Publish(new BanTokenCommand(jti, expires.AddMinutes(1)));

        (string token, DateTime newExpires) = generateResult.Value!;
        return new AuthenticationResult(user, token, newExpires);
    }

}
