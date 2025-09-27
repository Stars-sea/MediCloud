using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts.Result;

public record AddRecordCommandResult(
    RecordId RecordId,
    DateTime CreatedOn,
    string   ImageName
);
