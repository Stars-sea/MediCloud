using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record CreateLiveCommand(
    UserId UserId,
    string LiveName
) : Request<Result<LiveId>>;
