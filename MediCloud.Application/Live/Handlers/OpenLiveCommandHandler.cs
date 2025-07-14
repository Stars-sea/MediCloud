using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler(
    IPublishEndpoint                publishEndpoint,
    IOptions<LiveStreamingSettings> liveStreamingOption
) : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {
    
    private LiveStreamingSettings LiveStreamingSettings => liveStreamingOption.Value;

    public async Task<Result<OpenLiveCommandResult>> Handle(
        OpenLiveCommand                 request,
        ConsumeContext<OpenLiveCommand> ctx
    ) {
        string liveId = Guid.NewGuid().ToString();

        const string passphrase = ""; // TODO
        
        int timeout = LiveStreamingSettings.Timeout * 1000;
        int latency = LiveStreamingSettings.Latency * 1000;
        int ffs = LiveStreamingSettings.Ffs * 1000;
        
        PullStreamCommand command = new(
            liveId, $"{LiveStreamingSettings.SrtServer}?mode=listener",
            Path.Combine(LiveStreamingSettings.StoragePath, request.UserId),
            passphrase, timeout, latency, ffs
        );

        await publishEndpoint.Publish(command);

        return new OpenLiveCommandResult(
            liveId,
            request.LiveName,
            "", // TODO
            $"{LiveStreamingSettings.SrtServer}?mode=caller",
            passphrase
        );
    }

}
