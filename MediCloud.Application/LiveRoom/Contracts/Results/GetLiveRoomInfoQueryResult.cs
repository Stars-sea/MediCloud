using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.LiveRoom.Contracts.Results;

public sealed record GetLiveRoomInfoQueryResult(
    LiveRoomId     Id,
    UserId         OwnerId,
    string         RoomName,
    LiveRoomStatus Status,
    LiveId?        ActiveLiveId,
    LiveId?        PendingLiveId
);
