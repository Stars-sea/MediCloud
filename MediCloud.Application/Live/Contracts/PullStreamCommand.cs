namespace MediCloud.Application.Live.Contracts;

public record PullStreamCommand(
    string Id,
    string Url,
    string Passphrase,
    ulong  Timeout,
    ulong  Latency,
    ulong  Ffs,
    string Path,
    uint   SegmentTime,
    uint   ListSize,
    bool   DeleteSegments
);
