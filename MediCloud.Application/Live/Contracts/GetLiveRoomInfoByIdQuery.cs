using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record GetLiveRoomInfoByIdQuery(
    LiveRoomId LiveRoomId
) : Request<Result<GetLiveRoomInfoQueryResult>>;
