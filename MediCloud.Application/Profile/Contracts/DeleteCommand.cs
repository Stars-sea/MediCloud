using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;

namespace MediCloud.Application.Profile.Contracts;

public record DeleteCommand(
    string Username,
    string Email,
    string Password
) : Request<Result>;
