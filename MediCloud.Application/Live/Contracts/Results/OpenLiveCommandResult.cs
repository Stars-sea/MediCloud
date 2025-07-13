namespace MediCloud.Application.Live.Contracts.Results;

public record OpenLiveCommandResult(
    string LiveUrl,
    int    Timeout,
    int    Latency,
    int    Ffs
);
