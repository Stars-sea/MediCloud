using System.ComponentModel.DataAnnotations;
using MediCloud.Application.Authentication.Contracts;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Contracts.Authentication;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Handlers;

public class RegisterCommandHandler(
    IJwtTokenManager jwtTokenManager,
    IUserRepository  userRepository
) : IRequestHandler<RegisterCommand, Result<AuthenticationResult>> {

    public async Task<Result<AuthenticationResult>> Handle(RegisterCommand request) {
        if (await userRepository.FindByEmailAsync(request.Email) is not null)
            return Errors.User.DuplicateEmail;

        User user;  // TODO: Use FluentValidation
        try { user = User.Factory.Create(request.Email, request.Username); }
        catch (ValidationException) { return Errors.User.InvalidEmail; }
        catch (FormatException) { return Errors.User.InvalidUsername; }

        Result result = await userRepository.CreateAsync(user, request.Password);
        if (!result.IsSuccess) return result.Errors;

        Result<JwtGenerateResult> generateResult = jwtTokenManager.GenerateToken(user);
        if (!generateResult.IsSuccess) return generateResult.Errors;

        await userRepository.UpdateLastLoginDateAsync(user);

        (string token, DateTime expires) = generateResult.Value!;
        return new AuthenticationResult(user, token, expires);
    }

}
