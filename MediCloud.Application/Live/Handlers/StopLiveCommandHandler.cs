using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Common.Protos;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Live.Handlers;

public class StopLiveCommandHandler(
    ILiveManager                liveManager,
    ILiveRepository             liveRepository,
    Livestream.LivestreamClient livestreamClient
) : IRequestHandler<StopLiveCommand, Result> {

    public async Task<Result> Handle(StopLiveCommand request, ConsumeContext<StopLiveCommand> ctx) {
        if (await liveRepository.FindLiveById(request.LiveId) is not { } live ||
            live.OwnerId != request.UserId)
            return Errors.Live.LiveNotFound;

        try {
            await livestreamClient.StopPullStreamAsync(new StopPullStreamRequest {
                    LiveId = request.LiveId.ToString()
                }
            );
        }
        catch { return Errors.Live.LiveFailedToStop; }

        await liveManager.StopLiveAsync(live);
        return Result.Ok;
    }

}
