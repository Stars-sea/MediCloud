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
    IOptions<LivestreamSettings> liveStreamingOption
) : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {

    private LivestreamSettings LivestreamSettings => liveStreamingOption.Value;

    private async ValueTask<StartPullStreamResponse> StartPullStreamAsync(string url, string passphrase, LiveId liveId) {
        return await livestreamClient.StartPullStreamAsync(new StartPullStreamRequest {
                Url        = url,
                Passphrase = passphrase,
                LiveId     = liveId.ToString()
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
            return Errors.Live.LiveRoomNotFound;

        if (liveRoom.Status != LiveRoomStatus.Available)
            return Errors.Live.LiveFailedToStart;

        Result startResult = live.Start();
        if (!startResult.IsSuccess) return startResult.Errors;

        Result createResult = await liveRepository.CreateAsync(live);
        if (!createResult.IsSuccess) return createResult.Errors;

        const string passphrase = "";// TODO

        var resp = await StartPullStreamAsync(
            LivestreamSettings.SrtServer,
            passphrase,
            live.Id
        );

        return live.MapOpenLiveResult("...", resp.Url, resp.Code);
    }

}
