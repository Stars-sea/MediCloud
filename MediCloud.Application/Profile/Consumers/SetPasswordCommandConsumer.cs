using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Profile.Consumers;

public class SetPasswordCommandConsumer(
    IUserRepository userRepository
) : IRequestConsumer<SetPasswordCommand> {

    public async Task<Result> Consume(ConsumeContext<SetPasswordCommand> context) {
        SetPasswordCommand command = context.Message;

        if (await userRepository.FindByEmailAsync(command.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, command.OldPassword))
            return Errors.Auth.InvalidCred;

        return await userRepository.SetPasswordAsync(user, command.NewPassword);
    }

}
