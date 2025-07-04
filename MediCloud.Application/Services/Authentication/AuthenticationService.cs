using MediCloud.Application.Common.Interfaces.Authentication;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Entities;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace MediCloud.Application.Services.Authentication;

public class AuthenticationService(
    IJwtTokenGenerator jwtTokenGenerator,
    UserManager<User>  userManager
) : IAuthenticationService {
    public async Task<ErrorOr<AuthenticationResult>> RegisterAsync(string username, string email, string password) {
        if (await userManager.FindByEmailAsync(email) is not null)
            return Errors.User.DuplicateEmail;

        User user = new() {
            Email    = email,
            UserName = username,
        };

        await userManager.CreateAsync(user, password);

        string token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }

    public async Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password) {
        if (await userManager.FindByEmailAsync(email) is not { } user)
            return Errors.Auth.InvalidCred;

        string token = jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}
