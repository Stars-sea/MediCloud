using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.LiveRoom.Contracts;
using MediCloud.Application.LiveRoom.Contracts.Mappers;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.LiveRoom.Handlers;

public class GetLiveRoomInfoQueryHandlers(
    IUserRepository     userRepository,
    ILiveRoomRepository liveRoomRepository
) : IQueryHandler<GetLiveRoomInfoByIdQuery, Result<GetLiveRoomInfoQueryResult>>,
    IQueryHandler<GetLiveRoomInfoByOwnerIdQuery, Result<GetLiveRoomInfoQueryResult>> {

    private async Task<GetLiveRoomInfoQueryResult> MapResultAsync(Domain.LiveRoom.LiveRoom liveRoom) {
        Domain.Live.Live? activeLive  = await liveRoomRepository.FindActiveLiveInRoomAsync(liveRoom.Id);
        Domain.Live.Live? pendingLive = await liveRoomRepository.FindPendingLiveInRoomAsync(liveRoom.Id);
        return liveRoom.MapGetInfoResult(activeLive?.Id, pendingLive?.Id);
    }

    public async ValueTask<Result<GetLiveRoomInfoQueryResult>> Handle(GetLiveRoomInfoByIdQuery query, CancellationToken cancellationToken) {
        if (await liveRoomRepository.FindByIdAsync(query.LiveRoomId) is not { } liveRoom)
            return Errors.LiveRoom.LiveRoomNotFound;

        return await MapResultAsync(liveRoom);
    }

    public async ValueTask<Result<GetLiveRoomInfoQueryResult>> Handle(GetLiveRoomInfoByOwnerIdQuery query, CancellationToken cancellationToken) {
        if (await userRepository.FindByIdAsync(query.OwnerId) is not { } user)
            return Errors.User.UserNotFound;

        if (user.LiveRoomId == null || await liveRoomRepository.FindByIdAsync(user.LiveRoomId) is not { } liveRoom)
            return Errors.LiveRoom.LiveRoomNotFound;

        return await MapResultAsync(liveRoom);
    }

}
