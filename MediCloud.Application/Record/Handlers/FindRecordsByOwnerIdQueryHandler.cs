using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Mappers;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordsByOwnerIdQueryHandler(
    IRecordRepository recordRepository
) : IQueryHandler<FindRecordsByOwnerIdQuery, Result<List<FindRecordByIdQueryResult>>> {

    public async ValueTask<Result<List<FindRecordByIdQueryResult>>> Handle(FindRecordsByOwnerIdQuery query, CancellationToken cancellationToken) {
        List<Domain.Record.Record> records = await recordRepository.GetAllRecordsAsync(query.UserId);
        return records.MapFindRecordsByOwnerIdResult();
    }

}
