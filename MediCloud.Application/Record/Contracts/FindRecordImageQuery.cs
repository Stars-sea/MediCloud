using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record FindRecordImageQuery(
    UserId UserId,
    string ImageName
) : Request<Result<Stream>>;
