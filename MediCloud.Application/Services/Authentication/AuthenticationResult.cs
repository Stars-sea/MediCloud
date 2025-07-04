using MediCloud.Domain.Entities;

namespace MediCloud.Application.Services.Authentication;

public record AuthenticationResult(
    User   User,
    string Token
);
