namespace MediCloud.Contracts.Live;

public sealed record LiveInfoResponse(
    string     LiveId,
    string     RoomId,
    string     OwnerId,
    string     LiveName,
    LiveStatus Status,
    DateTime?  StartedAt,
    DateTime?  EndedAt
);

public sealed record DetailedLiveInfoResponse(
    string     LiveId,
    string     RoomId,
    string     OwnerId,
    string     LiveName,
    LiveStatus Status,
    DateTime?  StartedAt,
    DateTime?  EndedAt,
    string?    PostUrl,
    string?    Passphrase
);
