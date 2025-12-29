using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Protos;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler(
    IUserRepository              userRepository,
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
        (string liveName, UserId userId) = request;

        if (await userRepository.FindByIdAsync(UserId.Factory.Create(userId)) is not { } user)
            return Errors.User.UserNotFound;

        if (await liveRoomRepository.GetLiveRoomFromOwnerAsync(user) is not { } liveRoom)
            return Errors.Live.LiveRoomNotFound;

        var createLiveResult = liveRoom.CreateLive(liveName);
        if (!createLiveResult.IsSuccess) return createLiveResult.Errors;

        var saveLiveRoomResult = await liveRoomRepository.SaveAsync();
        if (!saveLiveRoomResult.IsSuccess) return saveLiveRoomResult.Errors;

        Domain.Live.Live live = createLiveResult.Value!;

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

        return new OpenLiveCommandResult(live.Id.ToString(), liveName, "...", resp.Url, passphrase);
    }

}

