using MediCloud.Domain.Entities;

namespace MediCloud.Application.Authentication.Common;

public record AuthenticationResult(
    User   User,
    string Token
);
