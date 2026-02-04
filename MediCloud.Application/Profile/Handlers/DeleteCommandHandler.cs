using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Profile.Handlers;

public class DeleteCommandHandler(
    IUserRepository userRepository
) : ICommandHandler<DeleteCommand, Result> {

    public async ValueTask<Result> Handle(DeleteCommand command, CancellationToken cancellationToken) {
        if (await userRepository.FindByEmailAsync(command.Email) is not { } user ||
            !user.Username.Equals(command.Username, StringComparison.OrdinalIgnoreCase))
            return Errors.Auth.UsernameEmailNotMatch;

        if (!await userRepository.VerifyPasswordAsync(user, command.Password))
            return Errors.Auth.InvalidCred;

        Result dbResult = await userRepository.RemoveAsync(user) & await userRepository.SaveAsync();
        return !dbResult.IsSuccess ? Errors.Record.RecordFailedToDelete : Result.Ok;
    }

}
