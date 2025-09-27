namespace MediCloud.Contracts.Record;

public record FindRecordResponse(
    string         RecordId,
    string         OwnerId,
    string         ImageUrl,
    string         Remarks,
    DateTimeOffset CreatedOn
);
