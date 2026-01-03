using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Mappers;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Live.Handlers;

public class GetLiveRoomInfoQueryHandler(
    IUserRepository     userRepository,
    ILiveRoomRepository liveRoomRepository
) : IRequestHandler<GetLiveRoomInfoByIdQuery, Result<GetLiveRoomInfoQueryResult>>,
    IRequestHandler<GetLiveRoomInfoByOwnerIdQuery, Result<GetLiveRoomInfoQueryResult>> {

    public async Task<Result<GetLiveRoomInfoQueryResult>> Handle(
        GetLiveRoomInfoByIdQuery                 request,
        ConsumeContext<GetLiveRoomInfoByIdQuery> ctx
    ) {
        if (await liveRoomRepository.FindByIdAsync(request.LiveRoomId) is not { } liveRoom)
            return Errors.Live.LiveRoomNotFound;

        Domain.Live.Live? activeLive = await liveRoomRepository.FindActiveLiveInRoomAsync(liveRoom.Id);
        return liveRoom.MapGetInfoResult(activeLive?.Id);
    }

    public async Task<Result<GetLiveRoomInfoQueryResult>> Handle(
        GetLiveRoomInfoByOwnerIdQuery                 request,
        ConsumeContext<GetLiveRoomInfoByOwnerIdQuery> ctx
    ) {
        if (await userRepository.FindByIdAsync(request.OwnerId) is not { } user)
            return Errors.User.UserNotFound;

        if (user.LiveRoomId == null || await liveRoomRepository.FindByIdAsync(user.LiveRoomId) is not { } liveRoom)
            return Errors.Live.LiveRoomNotFound;

        Domain.Live.Live? activeLive = await liveRoomRepository.FindActiveLiveInRoomAsync(liveRoom.Id);
        return  liveRoom.MapGetInfoResult(activeLive?.Id);
    }

}
