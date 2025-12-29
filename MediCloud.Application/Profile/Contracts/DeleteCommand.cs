using MassTransit.Mediator;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Profile.Contracts;

public sealed record DeleteCommand(
    string Username,
    string Email,
    string Password
) : Request<Result>;
