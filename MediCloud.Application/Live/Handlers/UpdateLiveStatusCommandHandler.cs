using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Handlers;

public class UpdateLiveStatusCommandHandler(
    ILiveRepository liveRepository,
    IBus            bus
) : IRequestHandler<UpdateLiveStatusCommand, Result> {

    private async Task<Result> StartLiveAsync(Domain.Live.Live live, ConsumeContext<UpdateLiveStatusCommand> ctx) {
        var response = await ctx.Request<OpenLiveCommand, Result<OpenLiveCommandResult>>(
            bus,
            new OpenLiveCommand(live.LiveName, live.OwnerId)
        );

        return response.Message;
    }

    private async Task<Result> StopLiveAsync(Domain.Live.Live live, ConsumeContext<UpdateLiveStatusCommand> ctx) {
        var response = await ctx.Request<StopLiveCommand, Result>(
            bus,
            new StopLiveCommand(live.OwnerId, live.Id)
        );

        return response.Message;
    }

    private async Task<Result> RenameLiveAsync(Domain.Live.Live live, string liveName) {
        Result renameResult = live.Rename(liveName);
        if (!renameResult.IsSuccess) return renameResult.Errors;

        return await liveRepository.SaveAsync();
    }

    public async Task<Result> Handle(UpdateLiveStatusCommand request, ConsumeContext<UpdateLiveStatusCommand> ctx) {
        (UserId userId, LiveId liveId, string? liveName, LiveStatus? status) = request;

        Domain.Live.Live? live = await liveRepository.FindLiveById(liveId);
        if (live is null || live.OwnerId != userId)
            return Errors.Live.LiveNotFound;

        switch (status) {
            case null: break;
            case LiveStatus.Streaming:
            {
                Result startResult = await StartLiveAsync(live, ctx);
                if (!startResult.IsSuccess) return startResult.Errors;
                break;
            }
            case LiveStatus.Stopped:
            {
                Result stopResult = await StopLiveAsync(live, ctx);
                if (!stopResult.IsSuccess) return stopResult.Errors;
                break;
            }
            default:
                return Errors.Live.LiveInvalidStatus;
        }

        if (liveName == null) return Result.Ok;
        return await RenameLiveAsync(live, liveName);
    }

}
