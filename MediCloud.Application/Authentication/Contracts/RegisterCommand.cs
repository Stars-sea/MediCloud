using Mediator;
using MediCloud.Application.Authentication.Contracts.Results;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Authentication.Contracts;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
) : ICommand<Result<AuthenticationResult>>;
