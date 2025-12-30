using MassTransit.Mediator;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;
using MediCloud.Domain.Record.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public sealed record FindRecordByIdQuery(
    RecordId RecordId
) : Request<Result<FindRecordByIdQueryResult>>;
