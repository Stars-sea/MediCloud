using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Profile.Consumers;

public class DeleteCommandConsumer(
    IUserRepository userRepository
) : IRequestConsumer<DeleteCommand> {

    public async Task<Result> Consume(ConsumeContext<DeleteCommand> context) {
        DeleteCommand command = context.Message;

        if (await userRepository.FindByEmailAsync(command.Email) is not { } user ||
            !user.Username.Equals(command.Username, StringComparison.OrdinalIgnoreCase))
            return Errors.Auth.UsernameEmailNotMatch;

        if (!await userRepository.VerifyPasswordAsync(user, command.Password))
            return Errors.Auth.InvalidCred;

        await userRepository.DeleteAsync(user);
        return Result.Ok;
    }

}
