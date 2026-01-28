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
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.Enums;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler(
    ILiveRepository              liveRepository,
    ILiveRoomRepository          liveRoomRepository,
    Livestream.LivestreamClient  livestreamClient,
    IOptions<LivestreamSettings> livestreamSettings
) : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {

    private string SrtDomain => livestreamSettings.Value.SrtDomain;

    private async ValueTask<StartPullStreamResponse> StartPullStreamAsync(string passphrase, LiveId liveId) {
        return await livestreamClient.StartPullStreamAsync(new StartPullStreamRequest {
                LiveId     = liveId.ToString(),
                Passphrase = passphrase
            }
        );
    }

    public async Task<Result<OpenLiveCommandResult>> Handle(
        OpenLiveCommand                 request,
        ConsumeContext<OpenLiveCommand> ctx
    ) {
        LiveId liveId = request.LiveId;
        if (await liveRepository.FindLiveById(liveId) is not { } live ||
            live.OwnerId != request.UserId)
            return Errors.Live.LiveNotFound;

        if (await liveRoomRepository.FindByIdAsync(live.LiveRoomId) is not { } liveRoom)
            return Errors.LiveRoom.LiveRoomNotFound;

        if (liveRoom.Status != LiveRoomStatus.Pending)
            return Errors.Live.LiveFailedToStart;

        Result startResult = liveRoom.StartLive() & live.Start();
        if (!startResult.IsSuccess) return startResult.Errors;

        Result dbResult = await liveRepository.SaveAsync();
        if (!dbResult.IsSuccess) return dbResult.Errors;

        const string passphrase = "";// TODO

        var resp = await StartPullStreamAsync(
            passphrase,
            live.Id
        );

        return live.MapOpenLiveResult(SrtDomain, resp.Port, resp.Passphrase);
    }

}
