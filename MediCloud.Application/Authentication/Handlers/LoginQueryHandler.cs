using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Handlers;

public class LoginQueryHandler(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestHandler<LoginQuery, Result<AuthenticationResult>> {

    public async Task<Result<AuthenticationResult>> Handle(LoginQuery request, ConsumeContext<LoginQuery> ctx) {
        if (await userRepository.FindByEmailAsync(request.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, request.Password))
            return Errors.Auth.InvalidCred;

        Result<JwtGenerateResult> result = jwtTokenGenerator.GenerateToken(user);
        if (!result.IsSuccess) return result.Errors;

        user.UpdateLastLoginAt();
        await userRepository.SaveAsync();

        (string token, DateTimeOffset expires) = result.Value!;
        return new AuthenticationResult(user, token, expires);
    }

}
