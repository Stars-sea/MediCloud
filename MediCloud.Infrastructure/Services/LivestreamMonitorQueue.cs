using System.Threading.Channels;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Infrastructure.Services;

public class LivestreamMonitorQueue : ILivestreamMonitorQueue {
    
    private readonly Channel<LiveId> _liveIdChannel = Channel.CreateUnbounded<LiveId>();

    public ValueTask QueueLiveIdAsync(LiveId liveId, CancellationToken cancellationToken = default) {
        return _liveIdChannel.Writer.WriteAsync(liveId, cancellationToken);
    }

    public ValueTask<LiveId> DequeueLiveIdAsync(CancellationToken cancellationToken = default) {
        return _liveIdChannel.Reader.ReadAsync(cancellationToken);
    }

}
