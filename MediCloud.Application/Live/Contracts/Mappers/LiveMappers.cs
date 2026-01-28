using MediCloud.Application.Live.Contracts.Results;
using Riok.Mapperly.Abstractions;

namespace MediCloud.Application.Live.Contracts.Mappers;

[Mapper]
internal static partial class LiveMappers {

    [MapProperty(nameof(Domain.Live.Live.Id), nameof(GetLiveByIdQueryResult.LiveId))]
    [MapProperty(nameof(Domain.Live.Live.LiveRoomId), nameof(GetLiveByIdQueryResult.RoomId))]
    private static partial GetLiveByIdQueryResult MapGetStatusResultInternal(this Domain.Live.Live live, Uri? postUrl, string? passphrase);

    [MapperIgnoreSource(nameof(Domain.Live.Live.LiveRoomId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.OwnerId))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.Status))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.StartedAt))]
    [MapperIgnoreSource(nameof(Domain.Live.Live.EndedAt))]
    [MapProperty(nameof(Domain.Live.Live.Id), nameof(OpenLiveCommandResult.LiveId))]
    private static partial OpenLiveCommandResult MapOpenLiveResultInternal(this Domain.Live.Live live, Uri postUrl, string passphrase);

    private static Uri FormatSrtUrl(string srtDomain, uint port, string passphrase)
        => string.IsNullOrEmpty(passphrase)
            ? new Uri($"srt://{srtDomain}:{port}?mode=caller")
            : new Uri($"srt://{srtDomain}:{port}?mode=caller&passphrase={passphrase}");

    extension(Domain.Live.Live live) {

        public GetLiveByIdQueryResult MapGetStatusResult(string srtDomain, uint? port, string? passphrase) {
            if (port is null || passphrase is null)
                return live.MapGetStatusResultInternal(null, null);

            Uri postUrl = FormatSrtUrl(srtDomain, port.Value, passphrase);
            return live.MapGetStatusResultInternal(postUrl, passphrase);
        }

        public OpenLiveCommandResult MapOpenLiveResult(string srtDomain, uint port, string passphrase) {
            Uri postUrl = FormatSrtUrl(srtDomain, port, passphrase);
            return live.MapOpenLiveResultInternal(postUrl, passphrase);
        }

    }

}
