using Grpc.Core;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Common.Protos;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediCloud.Infrastructure.Services;

public sealed class LivestreamStatusSyncWorker(
    IServiceProvider                    serviceProvider,
    ILivestreamMonitorQueue             monitorQueue,
    ILogger<LivestreamStatusSyncWorker> logger
) : BackgroundService {

    private readonly Dictionary<LiveId, (Task, CancellationTokenSource)> _syncTasks = new();

    protected async override Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            var liveId = await monitorQueue.DequeueLiveIdAsync(stoppingToken);

            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            var task = SyncLiveStatus(liveId, cts.Token).ContinueWith(
                _ => _syncTasks.Remove(liveId),
                TaskContinuationOptions.ExecuteSynchronously
            );
            _syncTasks[liveId] = (task, cts);
        }
    }

    private async Task SyncLiveStatus(LiveId liveId, CancellationToken cancellationToken) {
        using var scope = serviceProvider.CreateScope();

        var livestreamService = scope.ServiceProvider.GetRequiredService<ILivestreamService>();

        var cts         = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var watchResult = livestreamService.WatchStreamStatusAsync(liveId, cts.Token);
        if (!watchResult.IsSuccess) {
            logger.LogError("Failed to watch stream {LiveId}: {Errors}", liveId, watchResult.Errors);
            return;
        }

        IAsyncStreamReader<WatchStreamStatusResponse> streamReader = watchResult.Value!;
        await foreach (var response in streamReader.ReadAllAsync(cancellationToken)) {
            if (response.IsStreaming) continue;
            
            logger.LogDebug("Stream {LiveId} is no longer streaming. Status synchronizing", liveId);

            await StopLiveAsync(liveId);
            await cts.CancelAsync();
            break;
        }
    }

    private async Task StopLiveAsync(LiveId liveId) {
        using var scope        = serviceProvider.CreateScope();
        var       liveRepo     = scope.ServiceProvider.GetRequiredService<ILiveRepository>();
        var       liveRoomRepo = scope.ServiceProvider.GetRequiredService<ILiveRoomRepository>();

        if (await liveRepo.FindLiveById(liveId) is not { } live)
            return;

        if (await liveRoomRepo.FindByIdAsync(live.LiveRoomId) is not { } liveRoom)
            return;

        Result result = live.Stop() & liveRoom.StopLive() & await liveRepo.SaveAsync();
        if (!result.IsSuccess) {
            logger.LogError("Failed to stop live stream {LiveId}: {Errors}", liveId, result.Errors);
        }
    }

    public async override Task StopAsync(CancellationToken cancellationToken) {
        foreach ((_, CancellationTokenSource cts) in _syncTasks.Values) {
            await cts.CancelAsync();
        }

        await Task.WhenAll(_syncTasks.Values.Select(x => x.Item1));
        await base.StopAsync(cancellationToken);
    }

}
