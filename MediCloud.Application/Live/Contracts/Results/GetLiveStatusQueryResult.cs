namespace MediCloud.Application.Live.Contracts.Results;

public record GetLiveStatusQueryResult(
    string LiveId,
    string OwnerId,
    string LiveStatus
);
