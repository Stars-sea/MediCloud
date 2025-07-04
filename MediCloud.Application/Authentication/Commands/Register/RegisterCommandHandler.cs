using ErrorOr;
using MediatR;
using MediCloud.Application.Authentication.Common;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediCloud.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    UserManager<User>  userManager
) : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>> {
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken) {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
            return Errors.User.DuplicateEmail;

        User user = new() {
            Email    = request.Email,
            UserName = request.Username,
        };

        await userManager.CreateAsync(user, request.Password);

        string token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}
