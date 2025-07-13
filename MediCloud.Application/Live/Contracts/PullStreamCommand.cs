namespace MediCloud.Application.Live.Contracts;

public record PullStreamCommand(
    string Id,
    string Url,
    string Path,
    string Passphrase,
    int    Timeout,
    int    Latency,
    int    Ffs
);
