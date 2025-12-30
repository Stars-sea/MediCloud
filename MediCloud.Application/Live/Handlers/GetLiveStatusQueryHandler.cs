using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Protos;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Mappers;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Live.Handlers;

public class GetLiveStatusQueryHandler(
    ILiveRepository             liveRepository,
    Livestream.LivestreamClient liveStreamClient
) : IRequestHandler<GetLiveStatusQuery, Result<GetLiveStatusQueryResult>> {

    private async ValueTask<GetStreamStatusResponse> GetLiveStatus(LiveId liveId) {
        return await liveStreamClient.GetStreamStatusAsync(new GetStreamStatusRequest {
            LiveId = liveId.ToString()
        });
    }

    public async Task<Result<GetLiveStatusQueryResult>> Handle(
        GetLiveStatusQuery                 request,
        ConsumeContext<GetLiveStatusQuery> ctx
    ) {
        if (await liveRepository.FindLiveById(request.LiveId) is not { } live)
            return Errors.Live.LiveNotFound;

        GetStreamStatusResponse response = await GetLiveStatus(live.Id);

        return live.MapGetStatusResult(response.Url, response.Code);
    }

}
