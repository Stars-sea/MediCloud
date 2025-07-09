using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Consumers;

public class LoginQueryConsumer(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestConsumer<LoginQuery, AuthenticationResult> {

    public async Task<Result<AuthenticationResult>> Consume(ConsumeContext<LoginQuery> context) {
        LoginQuery query = context.Message;
        
        if (await userRepository.FindByEmailAsync(query.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, query.Password))
            return Errors.Auth.InvalidCred;

        Result<JwtGenerateResult> result = jwtTokenGenerator.GenerateToken(user);
        if (!result.IsSuccess) return result.Errors;

        await userRepository.UpdateLastLoginDateAsync(user);
        
        (string token, DateTime expires) = result.Value!;
        return new AuthenticationResult(user, token, expires);
    }

}
