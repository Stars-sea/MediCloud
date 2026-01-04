using MediCloud.Domain.Live.Enums;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts.Results;

public sealed record GetLiveByIdQueryResult(
    LiveId     LiveId,
    LiveRoomId RoomId,
    UserId     OwnerId,
    string     LiveName,
    LiveStatus Status,
    DateTime?  StartedAt,
    DateTime?  EndedAt,
    string     PostUrl,
    string     Passphrase
);
