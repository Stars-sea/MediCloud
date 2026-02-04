using Mediator;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Profile.Contracts;

public sealed record SetPasswordCommand(
    string Email,
    string OldPassword,
    string NewPassword
) : ICommand<Result>;
