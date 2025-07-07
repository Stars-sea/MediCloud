using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Consumers;

public class RegisterCommandConsumer(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository    userRepository
) : IConsumer<RegisterCommand> {

    public async Task Consume(ConsumeContext<RegisterCommand> context) {
        RegisterCommand command = context.Message;

        if (await userRepository.FindByEmailAsync(command.Email) is not null) {
            await context.RespondAsync(new List<Error> {
                    Errors.User.DuplicateEmail
                }
            );
            return;
        }

        User user = User.Factory.Create(command.Email, command.Password);

        IList<Error> errors = await userRepository.CreateAsync(user, command.Password);
        if (errors.Any()) {
            await context.RespondAsync(errors);
            return;
        }

        string token = jwtTokenGenerator.GenerateToken(user);
        await context.RespondAsync(new AuthenticationResult(user, token));
    }

}
