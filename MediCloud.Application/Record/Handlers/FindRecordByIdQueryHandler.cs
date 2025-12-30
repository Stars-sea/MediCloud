using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Mappers;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordByIdQueryHandler(
    IRecordRepository recordRepository
) : IRequestHandler<FindRecordByIdQuery, Result<FindRecordByIdQueryResult>> {

    public async Task<Result<FindRecordByIdQueryResult>> Handle(FindRecordByIdQuery request, ConsumeContext<FindRecordByIdQuery> ctx) {
        var record = await recordRepository.FindRecordByIdAsync(request.RecordId);
        if (record is null) return Errors.Record.RecordNotFound;

        return record.MapFindRecordByIdResult();
    }

}
