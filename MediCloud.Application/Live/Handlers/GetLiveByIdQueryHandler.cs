using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Mappers;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.Enums;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class GetLiveByIdQueryHandler(
    ILiveRepository              liveRepository,
    ILivestreamService           livestreamService,
    IOptions<LivestreamSettings> livestreamSettings
) : IRequestHandler<GetLiveByIdQuery, Result<GetLiveByIdQueryResult>> {

    private string SrtDomain => livestreamSettings.Value.SrtDomain;

    public async Task<Result<GetLiveByIdQueryResult>> Handle(
        GetLiveByIdQuery                 request,
        ConsumeContext<GetLiveByIdQuery> ctx
    ) {
        if (await liveRepository.FindLiveById(request.LiveId) is not { } live)
            return Errors.Live.LiveNotFound;

        // TODO: Sync status
        if (live.Status != LiveStatus.Streaming)
            return live.MapGetStatusResult(SrtDomain, null, null);

        var resp = await livestreamService.GetStreamStatusAsync(live.Id);
        return resp.Map(status => live.MapGetStatusResult(
            SrtDomain,
            status.Port,
            status.Passphrase
        ));
    }

}
