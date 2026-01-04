using MediCloud.Application.Live.Contracts.Results;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.Live.Contracts.Mappers;

[Mapper]
internal static partial class LiveMappers {

    [MapProperty(nameof(Domain.Live.Live.Id), nameof(GetLiveByIdQueryResult.LiveId))]
    [MapProperty(nameof(Domain.Live.Live.LiveRoomId), nameof(GetLiveByIdQueryResult.RoomId))]
    public static partial GetLiveByIdQueryResult MapGetStatusResult(this Domain.Live.Live live, string postUrl, string passphrase);

    [MapperIgnoreSource(nameof(Domain.Live.Live.LiveRoomId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.OwnerId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.Status))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.StartedAt))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.EndedAt))]
    [MapProperty(nameof(Domain.Live.Live.Id), nameof(OpenLiveCommandResult.LiveId))]
    public static partial OpenLiveCommandResult MapOpenLiveResult(this Domain.Live.Live live, string watchUrl, string postUrl, string passphrase);

}
