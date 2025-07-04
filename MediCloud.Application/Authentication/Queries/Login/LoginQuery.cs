using ErrorOr;
using MediatR;
using MediCloud.Application.Authentication.Common;

namespace MediCloud.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;
