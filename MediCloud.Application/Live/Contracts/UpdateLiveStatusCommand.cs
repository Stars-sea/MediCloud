using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.Enums;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record UpdateLiveStatusCommand(
    UserId      UserId,
    LiveId      LiveId,
    string?     LiveName,
    LiveStatus? Status
) : Request<Result>;
