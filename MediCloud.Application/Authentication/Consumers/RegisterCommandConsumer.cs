using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediCloud.Application.Authentication.Consumers;

public class RegisterCommandConsumer(
    IJwtTokenGenerator jwtTokenGenerator,
    UserManager<User>  userManager
) : IConsumer<RegisterCommand> {
    public async Task Consume(ConsumeContext<RegisterCommand> context) {
        RegisterCommand command = context.Message;

        if (await userManager.FindByEmailAsync(command.Email) is not null) {
            await context.RespondAsync(Errors.User.DuplicateEmail);
            return;
        }

        User user = new() {
            Email    = command.Email,
            UserName = command.Username,
        };

        IdentityResult result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded) {
            if (result.Errors.FirstOrDefault() is { } error)
                await context.RespondAsync(Error.Conflict(error.Code, error.Description));
            else
                await context.RespondAsync(Errors.User.RegistrationFailed);
            return;
        }

        string token = jwtTokenGenerator.GenerateToken(user);
        await context.RespondAsync(new AuthenticationResult(user, token));
    }
}
