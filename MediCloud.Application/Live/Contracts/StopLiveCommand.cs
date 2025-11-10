using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public record StopLiveCommand(
    UserId UserId,
    LiveId LiveId
) : Request<Result>;
