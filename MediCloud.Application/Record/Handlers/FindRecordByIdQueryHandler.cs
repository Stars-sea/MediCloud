using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Mappers;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordByIdQueryHandler(
    IRecordRepository recordRepository
) : IQueryHandler<FindRecordByIdQuery, Result<FindRecordByIdQueryResult>> {

    public async ValueTask<Result<FindRecordByIdQueryResult>> Handle(FindRecordByIdQuery query, CancellationToken cancellationToken) {
        var record = await recordRepository.FindRecordByIdAsync(query.RecordId);
        if (record is null) return Errors.Record.RecordNotFound;

        return record.MapFindRecordByIdResult();
    }

}
