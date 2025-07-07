using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Authentication.Consumers;

public class LoginQueryConsumer(
    IUserRepository    userRepository,
    IJwtTokenGenerator jwtTokenGenerator
) : IConsumer<LoginQuery> {

    public async Task Consume(ConsumeContext<LoginQuery> context) {
        LoginQuery query = context.Message;

        if (await userRepository.FindByEmailAsync(query.Email) is not { } user ||
            !await userRepository.CheckPasswordAsync(user, query.Password)) {
            await context.RespondAsync(new List<Error> {
                    Errors.Auth.InvalidCred
                }
            );
            return;
        }

        string token = jwtTokenGenerator.GenerateToken(user);
        await context.RespondAsync(new AuthenticationResult(user, token));
    }

}
