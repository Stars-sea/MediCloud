using MediCloud.Contracts.Live;

namespace MediCloud.Contracts.LiveRoom;

public sealed record SimpleLiveInfo(
    string          Id,
    string          LiveName,
    LiveStatus      Status,
    DateTimeOffset? StartedAt,
    DateTimeOffset? EndedAt
);
