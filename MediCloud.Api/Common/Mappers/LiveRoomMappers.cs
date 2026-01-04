using MediCloud.Application.LiveRoom.Contracts;
using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Contracts.LiveRoom;
using MediCloud.Domain.LiveRoom.ValueObjects;
using MediCloud.Domain.User.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Api.Common.Mappers;

[Mapper]
public static partial class LiveRoomMappers {

    public static LiveRoomId ToLiveRoomId(this Guid liveRoomId) => LiveRoomId.Factory.Create(liveRoomId);

    public static partial CreateLiveRoomCommand MapCommand(this CreateLiveRoomRequest request, UserId userId);

    public static GetLiveRoomInfoByIdQuery ToGetLiveRoomQuery(this LiveRoomId liveRoomId) => new(liveRoomId);

    public static GetLiveRoomInfoByOwnerIdQuery ToGetLiveRoomQuery(this UserId ownerId) => new(ownerId);

    public static GetLivesOfLiveRoomQuery ToGetLivesOfLiveRoomQuery(this LiveRoomId liveRoomId) => new(liveRoomId);
    
    public static GetLivesOfUserQuery ToGetLivesOfUserQuery(this UserId userId) => new(userId);

    public static partial GetLiveRoomInfoResponse MapResp(this GetLiveRoomInfoQueryResult result);
    
    public static partial GetLivesOfLiveRoomResponse MapResp(this GetLivesOfLiveRoomQueryResult result);

}
