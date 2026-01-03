using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.LiveRoom;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.Live.Contracts.Mappers;

[Mapper]
internal static partial class LiveMappers {

    [MapProperty(nameof(Domain.Live.Live.Id), nameof(GetLiveStatusQueryResult.LiveId))]
    [MapProperty(nameof(Domain.Live.Live.LiveRoomId), nameof(GetLiveStatusQueryResult.RoomId))]
    public static partial GetLiveStatusQueryResult MapGetStatusResult(this Domain.Live.Live live, string postUrl, string passphrase);

    public static partial GetLiveRoomInfoQueryResult MapGetInfoResult(this LiveRoom liveRoom, LiveId? activeLiveId);

    [MapperIgnoreSource(nameof(Domain.Live.Live.LiveRoomId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.OwnerId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.Status))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.StartedAt))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.EndedAt))]
    [MapProperty(nameof(Domain.Live.Live.Id), nameof(OpenLiveCommandResult.LiveId))]
    public static partial OpenLiveCommandResult MapOpenLiveResult(this Domain.Live.Live live, string watchUrl, string postUrl, string passphrase);

}
