using MassTransit.Mediator;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Application.LiveRoom.Contracts;

public sealed record GetLiveRoomInfoByIdQuery(
    LiveRoomId LiveRoomId
) : Request<Result<GetLiveRoomInfoQueryResult>>;
