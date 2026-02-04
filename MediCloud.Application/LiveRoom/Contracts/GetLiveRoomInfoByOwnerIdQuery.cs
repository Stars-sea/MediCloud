using Mediator;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.LiveRoom.Contracts;

public sealed record GetLiveRoomInfoByOwnerIdQuery(
    UserId OwnerId
): IQuery<Result<GetLiveRoomInfoQueryResult>>;
