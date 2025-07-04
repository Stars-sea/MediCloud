using ErrorOr;
using MediatR;
using MediCloud.Application.Authentication.Common;

namespace MediCloud.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;
