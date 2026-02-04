using Mediator;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Mappers;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Handlers;

public class RegisterCommandHandler(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : ICommandHandler<RegisterCommand, Result<AuthenticationResult>> {

    public async ValueTask<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken) {
        if (await userRepository.FindByEmailAsync(command.Email) is not null)
            return Errors.User.DuplicateEmail;

        User user = User.Factory.Create(command.Email, command.Username);

        Result result = await userRepository.CreateAsync(user, command.Password) & await userRepository.SaveAsync();
        if (!result.IsSuccess) return result.Errors;

        Result<JwtGenerateResult> generateResult = jwtTokenGenerator.GenerateToken(user);
        if (!generateResult.IsSuccess) return generateResult.Errors;

        user.UpdateLastLoginAt();
        result = await userRepository.SaveAsync();
        if (!result.IsSuccess) return result.Errors;

        (string token, DateTimeOffset expires) = generateResult.Value!;
        return user.MapResult(token, expires);
    }

}
