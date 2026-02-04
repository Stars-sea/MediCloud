using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Profile.Handlers;

public class SetPasswordCommandHandler(
    IUserRepository userRepository
) : ICommandHandler<SetPasswordCommand, Result> {

    public async ValueTask<Result> Handle(SetPasswordCommand command, CancellationToken cancellationToken) {
        if (await userRepository.FindByEmailAsync(command.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, command.OldPassword))
            return Errors.Auth.InvalidCred;

        Result dbResult = await userRepository.SetPasswordAsync(user, command.NewPassword) & await userRepository.SaveAsync();
        return !dbResult.IsSuccess ? Errors.User.FailedToSave : Result.Ok;
    }

}
