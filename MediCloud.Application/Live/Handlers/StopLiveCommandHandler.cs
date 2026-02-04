using Mediator;
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
) : ICommandHandler<StopLiveCommand, Result> {

    public async ValueTask<Result> Handle(StopLiveCommand command, CancellationToken cancellationToken) {
        if (await liveRepository.FindLiveById(command.LiveId) is not { } live ||
            live.OwnerId != command.UserId)
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
