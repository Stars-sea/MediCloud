using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts.Result;

public sealed record AddRecordCommandResult(
    RecordId RecordId,
    DateTime CreatedOn
);
