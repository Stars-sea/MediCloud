namespace MediCloud.Contracts.Live;

public sealed record LiveStatusResponse(
    string     LiveId,
    string     RoomId,
    string     OwnerId,
    string     LiveName,
    LiveStatus Status,
    DateTime?  StartedAt,
    DateTime?  EndedAt
);

public sealed record DetailedLiveStatusResponse(
    string     LiveId,
    string     RoomId,
    string     OwnerId,
    string     LiveName,
    LiveStatus Status,
    DateTime?  StartedAt,
    DateTime?  EndedAt,
    string     PostUrl,
    string     Passphrase
);
