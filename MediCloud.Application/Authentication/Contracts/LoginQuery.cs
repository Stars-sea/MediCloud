using Mediator;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Authentication.Contracts;

public record LoginQuery(
    string Email,
    string Password
) : IQuery<Result<AuthenticationResult>>;
