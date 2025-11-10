using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Application.Common.Protos;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler(
    IUserRepository              userRepository,
    ILiveManager                 liveManager,
    Livestream.LivestreamClient  livestreamClient,
    IOptions<LivestreamSettings> liveStreamingOption
) : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {

    private LivestreamSettings LivestreamSettings => liveStreamingOption.Value;

    public async Task<Result<OpenLiveCommandResult>> Handle(
        OpenLiveCommand                 request,
        ConsumeContext<OpenLiveCommand> ctx
    ) {
        if (await userRepository.FindByIdAsync(UserId.Factory.Create(request.UserId)) is not { } user)
            return Errors.User.UserNotFound;

        if (await liveManager.GetLiveRoomFromOwnerAsync(user) is not { } liveRoom)
            return Errors.Live.LiveRoomNotFound;

        if (liveRoom.Status == LiveRoomStatus.Banned)
            return Errors.Live.LiveRoomBanned;

        Domain.Live.Live live = await liveManager.StartLiveAsync(liveRoom, request.LiveName);

        const string passphrase = ""; // TODO

        var resp = await livestreamClient.StartPullStreamAsync(new StartPullStreamRequest {
                Url        = LivestreamSettings.SrtServer,
                Passphrase = passphrase,
                LiveId     = live.Id.ToString()
            }
        );

        if (resp != null)
            return new OpenLiveCommandResult(live.Id.ToString(), request.LiveName, "...", resp.Url, passphrase);

        await liveManager.StopLiveAsync(live);
        return Errors.Live.LiveFailedToCreate;
    }

}
