using MediCloud.Domain.Entities;

namespace MediCloud.Application.Contracts.Authentication;

public record AuthenticationResult(
    User   User,
    string Token
);
