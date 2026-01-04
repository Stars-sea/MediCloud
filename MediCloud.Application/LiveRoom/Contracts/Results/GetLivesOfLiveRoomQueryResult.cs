using MediCloud.Domain.Live.Enums;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.ValueObjects;

namespace MediCloud.Application.LiveRoom.Contracts.Results;

public sealed record SimpleLiveInfo(
    LiveId     Id,
    string     LiveName,
    LiveStatus Status,
    DateTime?  StartedAt,
    DateTime?  EndedAt
);

public sealed record GetLivesOfLiveRoomQueryResult(
    LiveRoomId           LiveRoomId,
    List<SimpleLiveInfo> Lives
);
