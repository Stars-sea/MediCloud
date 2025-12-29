using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Live.Handlers;

public class GetLiveStatusQueryHandler(
    ILiveRepository liveRepository
) : IRequestHandler<GetLiveStatusQuery, Result<GetLiveStatusQueryResult>> {

    public async Task<Result<GetLiveStatusQueryResult>> Handle(
        GetLiveStatusQuery                 request,
        ConsumeContext<GetLiveStatusQuery> ctx
    ) {
        if (await liveRepository.FindLiveById(request.LiveId) is not { } live)
            return Errors.Live.LiveNotFound;

        return new GetLiveStatusQueryResult(
            live.Id.ToString(),
            live.OwnerId.ToString(),
            live.Status
        );
    }

}
