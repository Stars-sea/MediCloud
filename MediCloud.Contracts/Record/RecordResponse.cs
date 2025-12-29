namespace MediCloud.Contracts.Record;

public sealed record SimpleRecordResponse(
    string         Id,
    string         OwnerId,
    string         Title,
    DateTimeOffset CreatedOn
);

public sealed record RecordResponse(
    string              Id,
    string              OwnerId,
    string              Title,
    IEnumerable<string> Images,
    string              Remarks,
    DateTimeOffset      CreatedOn
);
