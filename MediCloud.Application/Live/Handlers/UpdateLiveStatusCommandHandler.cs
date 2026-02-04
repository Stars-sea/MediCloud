using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.Enums;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Handlers;

public class UpdateLiveStatusCommandHandler(
    IMediator       mediator,
    ILiveRepository liveRepository
) : ICommandHandler<UpdateLiveStatusCommand, Result> {

    private async Task<Result> RenameLiveAsync(Domain.Live.Live live, string liveName) {
        Result renameResult = live.Rename(liveName);
        if (!renameResult.IsSuccess) return renameResult.Errors;

        return await liveRepository.SaveAsync();
    }

    public async ValueTask<Result> Handle(UpdateLiveStatusCommand command, CancellationToken cancellationToken) {
        (UserId userId, LiveId liveId, string? liveName, LiveStatus? status) = command;

        Domain.Live.Live? live = await liveRepository.FindLiveById(liveId);
        if (live is null || live.OwnerId != userId)
            return Errors.Live.LiveNotFound;

        switch (status) {
            case null: break;
            case LiveStatus.Streaming:
            {
                Result startResult = await mediator.Send(new OpenLiveCommand(live.OwnerId, live.Id), cancellationToken);
                if (!startResult.IsSuccess) return startResult.Errors;
                break;
            }
            case LiveStatus.Stopped:
            {
                Result stopResult = await  mediator.Send(new StopLiveCommand(live.OwnerId, live.Id), cancellationToken);
                if (!stopResult.IsSuccess) return stopResult.Errors;
                break;
            }
            case LiveStatus.Pending:
            default:
                return Errors.Live.LiveInvalidStatus;
        }

        if (liveName == null) return Result.Ok;
        return await RenameLiveAsync(live, liveName);
    }

}
