using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record FindRecordByIdQuery(
    RecordId RecordId
) : Request<Result<Domain.Record.Record>>;
