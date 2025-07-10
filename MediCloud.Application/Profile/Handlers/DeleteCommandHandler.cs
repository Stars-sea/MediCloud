using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Profile.Handlers;

public class DeleteCommandHandler(
    IUserRepository userRepository
) : IRequestHandler<DeleteCommand> {

    public async Task<Result> Handle(DeleteCommand request, ConsumeContext<DeleteCommand> ctx) {
        if (await userRepository.FindByEmailAsync(request.Email) is not { } user ||
            !user.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase))
            return Errors.Auth.UsernameEmailNotMatch;

        if (!await userRepository.VerifyPasswordAsync(user, request.Password))
            return Errors.Auth.InvalidCred;

        await userRepository.DeleteAsync(user);
        return Result.Ok;
    }

}
