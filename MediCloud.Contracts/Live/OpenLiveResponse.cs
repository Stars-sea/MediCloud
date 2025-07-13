namespace MediCloud.Contracts.Live;

public record OpenLiveResponse(
    string LiveUrl,
    int    Timeout,
    int    Latency,
    int    Ffs
);
