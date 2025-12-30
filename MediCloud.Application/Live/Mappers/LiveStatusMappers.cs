using MediCloud.Application.Live.Contracts.Results;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.Live.Mappers;

[Mapper]
public static partial class LiveStatusMappers {

    [MapProperty(nameof(Domain.Live.Live.Id), nameof(GetLiveStatusQueryResult.LiveId))]
    [MapProperty(nameof(Domain.Live.Live.LiveRoomId), nameof(GetLiveStatusQueryResult.RoomId))]
    public static partial GetLiveStatusQueryResult MapResult(this Domain.Live.Live live, string postUrl, string passphrase);

}
