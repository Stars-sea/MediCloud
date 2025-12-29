namespace MediCloud.Contracts.Live;

public record UpdateLiveStatusResponse(
    string     LiveId,
    string     OwnerId,
    LiveStatus Status
);
