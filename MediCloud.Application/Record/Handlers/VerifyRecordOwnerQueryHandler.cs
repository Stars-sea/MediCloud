using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Record.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Handlers;

public class VerifyRecordOwnerQueryHandler(
    IRecordRepository recordRepository
) : IQueryHandler<VerifyRecordOwnerQuery, Result> {

    public async ValueTask<Result> Handle(VerifyRecordOwnerQuery query, CancellationToken cancellationToken) {
        (RecordId recordId, UserId userId) = query;

        Domain.Record.Record? record = await recordRepository.FindRecordByIdAsync(recordId);
        if (record is null || record.OwnerId == userId) return Errors.Record.RecordNotFound;

        return Result.Ok;
    }

}
