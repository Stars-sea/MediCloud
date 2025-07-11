using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Common.Interfaces.Authentication;

namespace MediCloud.Application.Authentication.Consumers;

public class BanTokenCommandConsumer(
    IJwtTokenBlacklist jwtTokenBlacklist
) : IConsumer<BanTokenCommand> {

    public Task Consume(ConsumeContext<BanTokenCommand> context) {
        (string jti, DateTimeOffset? banExpires) = context.Message;
        return jwtTokenBlacklist.BanTokenAsync(jti, banExpires);
    }

}
