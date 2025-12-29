namespace MediCloud.Contracts.Live;

public sealed record UpdateLiveStatusRequest(
    string     LiveName,
    LiveStatus Status
);
