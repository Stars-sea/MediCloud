using MassTransit.Mediator;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public sealed record FindRecordsByOwnerIdQuery(
    UserId UserId
) : Request<Result<List<FindRecordByIdQueryResult>>>;
