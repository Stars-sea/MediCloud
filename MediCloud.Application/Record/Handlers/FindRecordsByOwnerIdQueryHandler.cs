using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Mappers;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordsByOwnerIdQueryHandler(
    IRecordRepository recordRepository
) : IRequestHandler<FindRecordsByOwnerIdQuery, Result<List<FindRecordByIdQueryResult>>> {

    public async Task<Result<List<FindRecordByIdQueryResult>>> Handle(
        FindRecordsByOwnerIdQuery                 request,
        ConsumeContext<FindRecordsByOwnerIdQuery> ctx
    ) {
        List<Domain.Record.Record> records = await recordRepository.GetAllRecordsAsync(request.UserId);
        return records.MapFindRecordsByOwnerIdResult();
    }

}
