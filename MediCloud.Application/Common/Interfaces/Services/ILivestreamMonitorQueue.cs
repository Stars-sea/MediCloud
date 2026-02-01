using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Services;

public interface ILivestreamMonitorQueue {

    ValueTask QueueLiveIdAsync(LiveId liveId, CancellationToken cancellationToken = default);
    
    ValueTask<LiveId> DequeueLiveIdAsync(CancellationToken cancellationToken = default);

}
