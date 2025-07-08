using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Consumers;

public class RegisterCommandConsumer(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository    userRepository
) : IRequestConsumer<RegisterCommand, AuthenticationResult> {

    public async Task<Result<AuthenticationResult>> Consume(ConsumeContext<RegisterCommand> context) {
        RegisterCommand command = context.Message;

        if (await userRepository.FindByEmailAsync(command.Email) is not null)
            return Errors.User.DuplicateEmail;

        User user = User.Factory.Create(command.Email, command.Username);

        IList<Error> errors = await userRepository.CreateAsync(user, command.Password);
        if (errors.Any()) {
            return errors.ToArray();
        }

        string token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }

}
