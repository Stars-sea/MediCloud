using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Authentication.Contracts.Results;

public record AuthenticationResult(
    UserId         Id,
    string         Email,
    string         Username,
    string         Token,
    DateTimeOffset ExpiresAt
);
