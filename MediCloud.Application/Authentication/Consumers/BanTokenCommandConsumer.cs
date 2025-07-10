using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;

namespace MediCloud.Application.Authentication.Consumers;

public class BanTokenCommandConsumer(
    IJwtTokenManager jwtTokenManager
) : IConsumer<BanTokenCommand> {

    public Task Consume(ConsumeContext<BanTokenCommand> context) {
        return jwtTokenManager.BanTokenAsync(context.Message.Jti);
    }

}
