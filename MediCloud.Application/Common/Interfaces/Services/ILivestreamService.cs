using Grpc.Core;
using MediCloud.Application.Common.Protos;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Services;

public interface ILivestreamService {

    Task<Result<StartPullStreamResponse>> StartPullStreamAsync(LiveId liveId, string passphrase, CancellationToken cancellationToken = default);

    Task<Result<StopPullStreamResponse>> StopPullStreamAsync(LiveId liveId, CancellationToken cancellationToken = default);

    Task<Result<string[]>> ListActiveStreamsAsync(CancellationToken cancellationToken = default);

    Task<Result<GetStreamInfoResponse>> GetStreamStatusAsync(LiveId liveId, CancellationToken cancellationToken = default);

    Result<IAsyncStreamReader<WatchStreamStatusResponse>> WatchStreamStatusAsync(LiveId liveId, CancellationToken cancellationToken = default);

}
