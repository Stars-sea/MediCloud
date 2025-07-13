using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;

namespace MediCloud.Application.Live.Handlers;

public class OpenLiveCommandHandler : IRequestHandler<OpenLiveCommand, Result<OpenLiveCommandResult>> {

    public async Task<Result<OpenLiveCommandResult>> Handle(
        OpenLiveCommand                 request,
        ConsumeContext<OpenLiveCommand> ctx
    ) {
        // TODO
        const string liveUrl = "srt://192.168.31.55:4200";

        PullStreamCommand command = new(
            Guid.NewGuid().ToString(),
            $"{liveUrl}?mode=listener",
            $"./cache/{request.UserId}.ts",
            "",
            request.Timeout,
            request.Latency,
            request.Ffs
        );

        await ctx.Publish(command);

        return new OpenLiveCommandResult(
            $"{liveUrl}?mode=caller",
            request.Timeout,
            request.Latency,
            request.Ffs
        );
    }

}
