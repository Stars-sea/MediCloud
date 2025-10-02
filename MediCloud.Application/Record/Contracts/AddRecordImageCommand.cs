using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record AddRecordImageCommand(
    RecordId Id,
    Stream   Stream
) : Request<Result<string>>;
