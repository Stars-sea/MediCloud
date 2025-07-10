using MassTransit.Mediator;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Application.Common.Contracts;

namespace MediCloud.Application.Authentication.Contracts;

public record LoginQuery(
    string Email,
    string Password
) : Request<Result<AuthenticationResult>>;
