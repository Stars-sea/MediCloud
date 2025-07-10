using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Profile.Contracts;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Profile.Handlers;

public class SetPasswordCommandHandler(
    IUserRepository userRepository
) : IRequestHandler<SetPasswordCommand> {

    public async Task<Result> Handle(SetPasswordCommand request, ConsumeContext<SetPasswordCommand> ctx) {
        if (await userRepository.FindByEmailAsync(request.Email) is not { } user ||
            !await userRepository.VerifyPasswordAsync(user, request.OldPassword))
            return Errors.Auth.InvalidCred;

        return await userRepository.SetPasswordAsync(user, request.NewPassword);
    }

}
