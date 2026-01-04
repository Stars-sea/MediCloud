using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.LiveRoom.Contracts;
using MediCloud.Application.LiveRoom.Contracts.Mappers;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Common;

namespace MediCloud.Application.LiveRoom.Handlers;

public class GetLivesOfLiveRoomQueryHandler(
    ILiveRoomRepository liveRoomRepository
) : IRequestHandler<GetLivesOfLiveRoomQuery, Result<GetLivesOfLiveRoomQueryResult>> {

    public async Task<Result<GetLivesOfLiveRoomQueryResult>> Handle(
        GetLivesOfLiveRoomQuery                 request,
        ConsumeContext<GetLivesOfLiveRoomQuery> ctx
    ) {
        var lives = await liveRoomRepository.GetLivesFromLiveRoom(request.LiveRoomId).MapSimpleLiveInfoList();
        return new GetLivesOfLiveRoomQueryResult(
            request.LiveRoomId,
            lives
        );
    }

}
