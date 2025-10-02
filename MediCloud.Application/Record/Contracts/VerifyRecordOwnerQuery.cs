using MassTransit.Mediator;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record VerifyRecordOwnerQuery(
    RecordId RecordId,
    UserId   UserId
) : Request<Common.Contracts.Result>;
