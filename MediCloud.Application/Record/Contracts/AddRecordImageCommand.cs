using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public sealed record AddRecordImageCommand(
    RecordId Id,
    Stream   Stream
) : Request<Result<string>>;
