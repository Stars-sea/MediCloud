using Mediator;
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
) : IQueryHandler<GetLivesOfLiveRoomQuery, Result<GetLivesOfLiveRoomQueryResult>>,
    IQueryHandler<GetLivesOfUserQuery, Result<GetLivesOfLiveRoomQueryResult>> {

    public async ValueTask<Result<GetLivesOfLiveRoomQueryResult>> Handle(GetLivesOfLiveRoomQuery query, CancellationToken cancellationToken) {
        var lives = await liveRoomRepository.GetLivesFromLiveRoomId(query.LiveRoomId).MapSimpleLiveInfoList();
        return new GetLivesOfLiveRoomQueryResult(
            query.LiveRoomId,
            lives
        );
    }

    public async ValueTask<Result<GetLivesOfLiveRoomQueryResult>> Handle(GetLivesOfUserQuery query, CancellationToken cancellationToken) {
        User? user = await userRepository.FindByIdAsync(query.UserId);
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
