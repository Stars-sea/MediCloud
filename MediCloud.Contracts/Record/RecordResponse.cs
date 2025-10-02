namespace MediCloud.Contracts.Record;

public record SimpleRecordResponse(
    string         Id,
    string         OwnerId,
    string         Title,
    DateTimeOffset CreatedOn
);

public record RecordResponse(
    string              Id,
    string              OwnerId,
    string              Title,
    IEnumerable<string> Images,
    string              Remarks,
    DateTimeOffset      CreatedOn
) : SimpleRecordResponse(Id, OwnerId, Title, CreatedOn);
