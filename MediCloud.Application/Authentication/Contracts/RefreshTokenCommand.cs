using MassTransit.Mediator;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Authentication.Contracts;

public record RefreshTokenCommand(
    string Email,
    string Jti,
    string ExpiresStamp
) : Request<Result<AuthenticationResult>>;
