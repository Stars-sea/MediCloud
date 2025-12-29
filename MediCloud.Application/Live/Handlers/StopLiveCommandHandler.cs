using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Protos;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Live.Handlers;

public class StopLiveCommandHandler(
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

        Result result = live.Stop();
        if (!result.IsSuccess) return result.Errors;

        return await liveRepository.SaveAsync();
    }

}

