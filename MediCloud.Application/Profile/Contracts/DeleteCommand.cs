using MassTransit.Mediator;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Profile.Contracts;

public record DeleteCommand(
    string Username,
    string Email,
    string Password
) : Request<Result>;
