using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Contracts;

public record AuthenticationResult(
    User   User,
    string Token
);
