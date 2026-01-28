using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Live.Handlers;

public class StopLiveCommandHandler(
    ILiveRepository     liveRepository,
    ILiveRoomRepository liveRoomRepository,
    ILivestreamService  livestreamService
) : IRequestHandler<StopLiveCommand, Result> {

    public async Task<Result> Handle(StopLiveCommand request, ConsumeContext<StopLiveCommand> ctx) {
        if (await liveRepository.FindLiveById(request.LiveId) is not { } live ||
            live.OwnerId != request.UserId)
            return Errors.Live.LiveNotFound;

        if (await liveRoomRepository.FindByIdAsync(live.LiveRoomId) is not { } liveRoom)
            return Errors.LiveRoom.LiveRoomNotFound;

        var stopResult = await livestreamService.StopPullStreamAsync(live.Id);
        if (!stopResult.IsSuccess) return Errors.Live.LiveFailedToStop;

        Result result = live.Stop() & liveRoom.StopLive();
        if (!result.IsSuccess) return result.Errors;

        return await liveRepository.SaveAsync();
    }

}
