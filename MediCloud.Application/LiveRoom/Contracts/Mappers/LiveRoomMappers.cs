using MediCloud.Application.LiveRoom.Contracts.Results;
using MediCloud.Domain.Live.ValueObjects;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.LiveRoom.Contracts.Mappers;

[Mapper]
internal static partial class LiveRoomMappers {

    public static partial GetLiveRoomInfoQueryResult MapGetInfoResult(
        this Domain.LiveRoom.LiveRoom liveRoom,
        LiveId?                       activeLiveId,
        LiveId?                       pendingLiveId
    );

}
