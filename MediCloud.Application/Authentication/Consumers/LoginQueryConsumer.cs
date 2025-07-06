using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediCloud.Application.Authentication.Consumers;

public class LoginQueryConsumer(
    UserManager<User>  userManager,
    IJwtTokenGenerator jwtTokenGenerator
) : IConsumer<LoginQuery> {
    public async Task Consume(ConsumeContext<LoginQuery> context) {
        LoginQuery query = context.Message;

        if (await userManager.FindByEmailAsync(query.Email) is not { } user ||
            !await userManager.CheckPasswordAsync(user, query.Password)) {
            await context.RespondAsync(Errors.Auth.InvalidCred);
            return;
        }

        string token = jwtTokenGenerator.GenerateToken(user);
        await context.RespondAsync(new AuthenticationResult(user, token));
    }
}
