using MassTransit;
using MediCloud.Application.Common.Interfaces;
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
) : IRequestHandler<GetLiveRoomInfoByIdQuery, Result<GetLiveRoomInfoQueryResult>>,
    IRequestHandler<GetLiveRoomInfoByOwnerIdQuery, Result<GetLiveRoomInfoQueryResult>> {

    private async Task<GetLiveRoomInfoQueryResult> MapResultAsync(Domain.LiveRoom.LiveRoom liveRoom) {
        Domain.Live.Live? activeLive  = await liveRoomRepository.FindActiveLiveInRoomAsync(liveRoom.Id);
        Domain.Live.Live? pendingLive = await liveRoomRepository.FindPendingLiveInRoomAsync(liveRoom.Id);
        return liveRoom.MapGetInfoResult(activeLive?.Id, pendingLive?.Id);
    }

    public async Task<Result<GetLiveRoomInfoQueryResult>> Handle(
        GetLiveRoomInfoByIdQuery                 request,
        ConsumeContext<GetLiveRoomInfoByIdQuery> ctx
    ) {
        if (await liveRoomRepository.FindByIdAsync(request.LiveRoomId) is not { } liveRoom)
            return Errors.LiveRoom.LiveRoomNotFound;

        return await MapResultAsync(liveRoom);
    }

    public async Task<Result<GetLiveRoomInfoQueryResult>> Handle(
        GetLiveRoomInfoByOwnerIdQuery                 request,
        ConsumeContext<GetLiveRoomInfoByOwnerIdQuery> ctx
    ) {
        if (await userRepository.FindByIdAsync(request.OwnerId) is not { } user)
            return Errors.User.UserNotFound;

        if (user.LiveRoomId == null || await liveRoomRepository.FindByIdAsync(user.LiveRoomId) is not { } liveRoom)
            return Errors.LiveRoom.LiveRoomNotFound;

        return await MapResultAsync(liveRoom);
    }

}
