namespace MediCloud.Contracts.Live;

public record UpdateLiveStatusRequest(
    string     LiveName,
    LiveStatus Status
);
