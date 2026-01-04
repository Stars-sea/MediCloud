namespace MediCloud.Contracts.LiveRoom;

public sealed record GetLivesOfLiveRoomResponse(
    string               LiveRoomId,
    List<SimpleLiveInfo> Lives
);
