using ErrorOr;
using MediatR;
using MediCloud.Application.Authentication.Common;
using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediCloud.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    UserManager<User>  userManager,
    IJwtTokenGenerator jwtTokenGenerator
) : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>> {
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken) {
        if (await userManager.FindByEmailAsync(request.Email) is not { } user ||
            !await userManager.CheckPasswordAsync(user, request.Password))
            return Errors.Auth.InvalidCred;

        string token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}
