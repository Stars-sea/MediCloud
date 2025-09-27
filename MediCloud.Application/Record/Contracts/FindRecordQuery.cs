using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record FindRecordQuery(
    RecordId RecordId
) : Request<Result<Domain.Record.Record>>;
