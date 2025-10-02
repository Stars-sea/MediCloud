using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordByIdQueryHandler(
    IRecordRepository recordRepository
) : IRequestHandler<FindRecordByIdQuery, Result<Domain.Record.Record>> {

    public async Task<Result<Domain.Record.Record>> Handle(FindRecordByIdQuery request, ConsumeContext<FindRecordByIdQuery> ctx) {
        var record = await recordRepository.FindRecordByIdAsync(request.RecordId);
        if (record is null) return Errors.Record.RecordNotFound;

        return record;
    }

}
