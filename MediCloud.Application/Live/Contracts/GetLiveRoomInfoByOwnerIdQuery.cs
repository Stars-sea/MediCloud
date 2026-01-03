using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record GetLiveRoomInfoByOwnerIdQuery(
    UserId OwnerId
): Request<Result<GetLiveRoomInfoQueryResult>>;
