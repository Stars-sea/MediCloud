using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Handlers;

public class LoginQueryHandler(
    IUserRepository  userRepository,
    IJwtTokenManager jwtTokenManager
) : IRequestHandler<LoginQuery, Result<AuthenticationResult>> {

    public async Task<Result<AuthenticationResult>> Handle(LoginQuery request) {
        if (await userRepository.FindByEmailAsync(request.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, request.Password))
            return Errors.Auth.InvalidCred;

        Result<JwtGenerateResult> result = jwtTokenManager.GenerateToken(user);
        if (!result.IsSuccess) return result.Errors;

        await userRepository.UpdateLastLoginDateAsync(user);

        (string token, DateTime expires) = result.Value!;
        return new AuthenticationResult(user, token, expires);
    }

}
