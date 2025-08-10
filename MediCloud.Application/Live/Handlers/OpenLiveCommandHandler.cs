using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.LiveRoom.Enums;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler(
    IUserRepository                 userRepository,
    ILiveManager                    liveManager,
    IPublishEndpoint                publishEndpoint,
    IOptions<LiveStreamingSettings> liveStreamingOption
) : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {

    private LiveStreamingSettings LiveStreamingSettings => liveStreamingOption.Value;

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

        ulong timeout = LiveStreamingSettings.Timeout * 1000UL;
        ulong latency = LiveStreamingSettings.Latency * 1000UL;
        ulong ffs     = LiveStreamingSettings.Ffs * 1000UL;

        PullStreamCommand command = new(
            live.Id.ToString(), $"{LiveStreamingSettings.SrtServer}?mode=listener",
            passphrase, timeout, latency, ffs,
            Path.Combine(LiveStreamingSettings.StoragePath, live.Id.ToString()),
            5, 10, false
        );

        await publishEndpoint.Publish(command);

        return new OpenLiveCommandResult(
            live.Id.ToString(),
            request.LiveName,
            "", // TODO
            $"{LiveStreamingSettings.SrtServer}?mode=caller",
            passphrase
        );
    }

}
