using MassTransit.Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record FindRecordsByOwnerIdQuery(
    UserId UserId
) : Request<Result<List<Domain.Record.Record>>>;
