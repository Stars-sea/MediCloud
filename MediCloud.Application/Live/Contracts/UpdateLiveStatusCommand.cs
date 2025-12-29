using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public record UpdateLiveStatusCommand(
    UserId      UserId,
    LiveId      LiveId,
    string?     LiveName,
    LiveStatus? LiveStatus
) : Request<Result>;
