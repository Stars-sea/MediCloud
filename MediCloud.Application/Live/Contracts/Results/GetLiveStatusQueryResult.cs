using MediCloud.Domain.Live;

namespace MediCloud.Application.Live.Contracts.Results;

public record GetLiveStatusQueryResult(
    string     LiveId,
    string     OwnerId,
    LiveStatus Status
    // TODO: Add more fields
);
