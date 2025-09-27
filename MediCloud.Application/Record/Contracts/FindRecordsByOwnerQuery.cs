using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record FindRecordsByOwnerQuery(
    UserId UserId
) : Request<Result<List<Domain.Record.Record>>>;
