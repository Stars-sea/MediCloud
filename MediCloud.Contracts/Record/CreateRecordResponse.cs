namespace MediCloud.Contracts.Record;

public record CreateRecordResponse(
    string         RecordId,
    DateTimeOffset CreatedOn
);
