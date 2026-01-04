using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.LiveRoom.Contracts;
using MediCloud.Application.LiveRoom.Contracts.Mappers;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User;

namespace MediCloud.Application.LiveRoom.Handlers;

public class GetLivesQueryHandlers(
    IUserRepository     userRepository,
    ILiveRoomRepository liveRoomRepository
) : IRequestHandler<GetLivesOfLiveRoomQuery, Result<GetLivesOfLiveRoomQueryResult>>,
    IRequestHandler<GetLivesOfUserQuery, Result<GetLivesOfLiveRoomQueryResult>> {

    public async Task<Result<GetLivesOfLiveRoomQueryResult>> Handle(
        GetLivesOfLiveRoomQuery                 request,
        ConsumeContext<GetLivesOfLiveRoomQuery> ctx
    ) {
        var lives = await liveRoomRepository.GetLivesFromLiveRoomId(request.LiveRoomId).MapSimpleLiveInfoList();
        return new GetLivesOfLiveRoomQueryResult(
            request.LiveRoomId,
            lives
        );
    }

    public async Task<Result<GetLivesOfLiveRoomQueryResult>> Handle(
        GetLivesOfUserQuery                 request,
        ConsumeContext<GetLivesOfUserQuery> ctx
    ) {
        User? user = await userRepository.FindByIdAsync(request.UserId);
        if (user is null) return Errors.User.UserNotFound;

        var liveRoom = await liveRoomRepository.FindByOwnerAsync(user);
        if (liveRoom is null) return Errors.LiveRoom.LiveRoomNotFound;

        var lives = await liveRoomRepository.GetLivesFromLiveRoomId(liveRoom.Id).MapSimpleLiveInfoList();
        return new GetLivesOfLiveRoomQueryResult(
            liveRoom.Id,
            lives
        );
    }

}
