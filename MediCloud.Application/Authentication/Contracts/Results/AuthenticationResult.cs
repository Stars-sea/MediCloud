using MediCloud.Domain.User;

namespace MediCloud.Application.Authentication.Contracts.Results;

public record AuthenticationResult(
    User   User,
    string Token
);
