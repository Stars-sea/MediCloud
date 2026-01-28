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
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom.Enums;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler(
    ILiveRepository              liveRepository,
    ILiveRoomRepository          liveRoomRepository,
    ILivestreamService           livestreamService,
    IOptions<LivestreamSettings> livestreamSettings
) : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {

    private string SrtDomain => livestreamSettings.Value.SrtDomain;

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

        const string passphrase = "";// TODO

        var resp = await livestreamService.StartPullStreamAsync(liveId, passphrase);
        if (!resp.IsSuccess) return Errors.Live.LiveFailedToStart;

        Result startResult = liveRoom.StartLive() & live.Start();
        if (!startResult.IsSuccess) return startResult.Errors;

        Result dbResult = await liveRepository.SaveAsync();
        if (!dbResult.IsSuccess) return dbResult.Errors;

        return resp.Map(success => live.MapOpenLiveResult(SrtDomain, success.Port, success.Passphrase));
    }

}
