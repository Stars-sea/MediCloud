namespace MediCloud.Contracts.Live;

public sealed record LiveStatusResponse(
    string     LiveId,
    string     OwnerId,
    LiveStatus Status
);
