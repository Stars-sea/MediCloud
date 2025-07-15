using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Handlers;

public class RegisterCommandHandler(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestHandler<RegisterCommand, Result<AuthenticationResult>> {

    public async Task<Result<AuthenticationResult>> Handle(
        RegisterCommand                 request,
        ConsumeContext<RegisterCommand> ctx
    ) {
        if (await userRepository.FindByEmailAsync(request.Email) is not null)
            return Errors.User.DuplicateEmail;

        User user = User.Factory.Create(request.Email, request.Username);

        Result result = await userRepository.CreateAsync(user, request.Password);
        if (!result.IsSuccess) return result.Errors;

        Result<JwtGenerateResult> generateResult = jwtTokenGenerator.GenerateToken(user);
        if (!generateResult.IsSuccess) return generateResult.Errors;

        user.UpdateLastLoginAt();
        await userRepository.UpdateAsync(user);

        (string token, DateTimeOffset expires) = generateResult.Value!;
        return new AuthenticationResult(user, token, expires);
    }

}
