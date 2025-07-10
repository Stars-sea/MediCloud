using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;

namespace MediCloud.Application.Authentication.Consumers;

public class UnbanTokenCommandConsumer(
    IJwtTokenManager jwtTokenManager
) : IConsumer<UnbanTokenCommand> {

    public Task Consume(ConsumeContext<UnbanTokenCommand> context) {
        return jwtTokenManager.UnbanTokenAsync(context.Message.Jti);
    }

}
