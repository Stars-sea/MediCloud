using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Protos;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Mappers;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.Enums;
using MediCloud.Domain.Live.ValueObjects;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class GetLiveByIdQueryHandler(
    ILiveRepository              liveRepository,
    Livestream.LivestreamClient  liveStreamClient,
    IOptions<LivestreamSettings> livestreamSettings
) : IRequestHandler<GetLiveByIdQuery, Result<GetLiveByIdQueryResult>> {
    
    private string SrtDomain => livestreamSettings.Value.SrtDomain;

    private async ValueTask<GetStreamStatusResponse?> GetLiveStatus(LiveId liveId) {
        try {
            return await liveStreamClient.GetStreamStatusAsync(new GetStreamStatusRequest {
                LiveId = liveId.ToString()
            });
        }
        catch (Exception e) {
            // TODO: Log exception
            return null;
        }
    }

    public async Task<Result<GetLiveByIdQueryResult>> Handle(
        GetLiveByIdQuery                 request,
        ConsumeContext<GetLiveByIdQuery> ctx
    ) {
        if (await liveRepository.FindLiveById(request.LiveId) is not { } live)
            return Errors.Live.LiveNotFound;

        GetStreamStatusResponse? response = null;
        if (live.Status == LiveStatus.Streaming) {
            response = await GetLiveStatus(request.LiveId);
        }

        // TODO: Sync status
        return live.MapGetStatusResult(
            SrtDomain,
            response?.Port,
            response?.Passphrase
        );
    }

}
