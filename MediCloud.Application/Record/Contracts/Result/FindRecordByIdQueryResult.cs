namespace MediCloud.Application.Record.Contracts.Result;

public sealed record FindRecordByIdQueryResult(
    string              Id,
    string              OwnerId,
    string              Title,
    IEnumerable<string> ImageUrls,
    string              Remarks,
    DateTimeOffset      CreatedOn
);
