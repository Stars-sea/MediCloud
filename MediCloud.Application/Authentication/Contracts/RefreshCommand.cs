using MassTransit.Mediator;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;

namespace MediCloud.Application.Authentication.Contracts;

public record RefreshCommand(
    string Email,
    string Jti,
    string ExpiresStamp
) : Request<Result<AuthenticationResult>>;
