using System.ComponentModel.DataAnnotations;
using MassTransit;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Contracts;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Consumers;

public class RegisterCommandConsumer(
    IJwtTokenGenerator jwtTokenGenerator,
    IUserRepository    userRepository
) : IRequestConsumer<RegisterCommand, AuthenticationResult> {

    public async Task<Result<AuthenticationResult>> Consume(ConsumeContext<RegisterCommand> context) {
        RegisterCommand command = context.Message;

        if (await userRepository.FindByEmailAsync(command.Email) is not null)
            return Errors.User.DuplicateEmail;

        User user;
        try { user = User.Factory.Create(command.Email, command.Username); }
        catch (ValidationException) { return Errors.User.InvalidEmail; }
        catch (FormatException) { return Errors.User.InvalidUsername; }

        Result result = await userRepository.CreateAsync(user, command.Password);
        if (!result.IsSuccess) return result.Errors;

        string token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }

}
