using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordsByOwnerIdQueryHandler(
    IRecordRepository recordRepository
) : IRequestHandler<FindRecordsByOwnerIdQuery, Result<List<Domain.Record.Record>>> {

    public async Task<Result<List<Domain.Record.Record>>> Handle(
        FindRecordsByOwnerIdQuery                 request,
        ConsumeContext<FindRecordsByOwnerIdQuery> ctx
    ) {
        return await recordRepository.GetAllRecordsAsync(request.UserId);
    }

}
