namespace MediCloud.Contracts.Record;

public sealed record CreateRecordResponse(
    string         RecordId,
    DateTimeOffset CreatedOn
);
