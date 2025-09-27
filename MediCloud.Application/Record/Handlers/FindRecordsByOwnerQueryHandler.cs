using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordsByOwnerQueryHandler(
    IRecordRepository recordRepository
) : IRequestHandler<FindRecordsByOwnerQuery, Result<List<Domain.Record.Record>>> {

    public async Task<Result<List<Domain.Record.Record>>> Handle(
        FindRecordsByOwnerQuery                 request,
        ConsumeContext<FindRecordsByOwnerQuery> ctx
    ) {
        return await recordRepository.GetAllRecords(request.UserId);
    }

}
