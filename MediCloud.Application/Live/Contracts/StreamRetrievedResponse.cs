namespace MediCloud.Application.Live.Contracts;

public record StreamRetrievedResponse(
    string LiveId,
    string Url,
    string Path,
    string Code
);
