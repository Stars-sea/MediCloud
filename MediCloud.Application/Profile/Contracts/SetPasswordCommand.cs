using MassTransit.Mediator;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Profile.Contracts;

public sealed record SetPasswordCommand(
    string Email,
    string OldPassword,
    string NewPassword
) : Request<Result>;
