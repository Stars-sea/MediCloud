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

    [MapperIgnoreSource(nameof(Domain.Live.Live.OwnerId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.LiveRoomId))]
    private static partial SimpleLiveInfo MapSimpleLiveInfo(this Domain.Live.Live live);

    public static ValueTask<List<SimpleLiveInfo>> MapSimpleLiveInfoList(this IAsyncEnumerable<Domain.Live.Live> lives) {
        return lives.Select(l => l.MapSimpleLiveInfo()).ToListAsync();
    }

}
