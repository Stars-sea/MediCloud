using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;

namespace MediCloud.Application.Authentication.Consumers;

public class UnbanTokenCommandConsumer(
    IJwtTokenBlacklist jwtTokenBlacklist
) : IConsumer<UnbanTokenCommand> {

    public Task Consume(ConsumeContext<UnbanTokenCommand> context) {
        return jwtTokenBlacklist.UnbanTokenAsync(context.Message.Jti);
    }

}
