using MassTransit.Mediator;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Application.LiveRoom.Contracts;

public sealed record GetLivesOfLiveRoomQuery(
    LiveRoomId LiveRoomId
) : Request<Result<GetLivesOfLiveRoomQueryResult>>;
