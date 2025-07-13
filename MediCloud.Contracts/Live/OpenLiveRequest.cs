namespace MediCloud.Contracts.Live;

public record OpenLiveRequest(
    int Timeout,
    int Latency,
    int Ffs
);
