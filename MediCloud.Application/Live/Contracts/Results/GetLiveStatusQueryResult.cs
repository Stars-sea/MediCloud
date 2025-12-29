using MediCloud.Domain.Live;

namespace MediCloud.Application.Live.Contracts.Results;

public sealed record GetLiveStatusQueryResult(
    string     LiveId,
    string     OwnerId,
    LiveStatus Status
    // TODO: Add more fields
);
