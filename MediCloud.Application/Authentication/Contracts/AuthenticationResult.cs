using MediCloud.Domain.Entities;

namespace MediCloud.Application.Authentication.Contracts;

public record AuthenticationResult(
    User   User,
    string Token
);
