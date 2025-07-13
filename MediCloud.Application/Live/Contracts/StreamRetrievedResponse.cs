namespace MediCloud.Application.Live.Contracts;

public record StreamRetrievedResponse(
    string Id,
    string Url,
    string Path,
    string Code
);
