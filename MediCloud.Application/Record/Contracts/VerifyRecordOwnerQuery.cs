using Mediator;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public sealed record VerifyRecordOwnerQuery(
    RecordId RecordId,
    UserId   UserId
) : IQuery<Domain.Common.Result>;
