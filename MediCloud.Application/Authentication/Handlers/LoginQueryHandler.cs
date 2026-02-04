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

public class LoginQueryHandler(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : IQueryHandler<LoginQuery, Result<AuthenticationResult>> {

    public async ValueTask<Result<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken) {
        if (await userRepository.FindByEmailAsync(query.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, query.Password))
            return Errors.Auth.InvalidCred;

        Result<JwtGenerateResult> result = jwtTokenGenerator.GenerateToken(user);
        if (!result.IsSuccess) return result.Errors;

        user.UpdateLastLoginAt();
        await userRepository.SaveAsync();

        (string token, DateTimeOffset expires) = result.Value!;
        return user.MapResult(token, expires);
    }

}
