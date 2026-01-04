namespace MediCloud.Contracts.LiveRoom;

public record GetLiveRoomInfoResponse(
    string         Id,
    string         OwnerId,
    string         RoomName,
    LiveRoomStatus Status,
    string?        ActiveLiveId,
    string?        PendingLiveId
);
