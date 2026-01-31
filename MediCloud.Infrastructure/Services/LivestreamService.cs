using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Application.Common.Protos;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Live.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MediCloud.Infrastructure.Services;

[SuppressMessage("Performance", "CA1873:避免进行可能成本高昂的日志记录")]
public class LivestreamService(
    Livestream.LivestreamClient grpcClient,
    ILogger<LivestreamService>  logger
) : ILivestreamService {

    public async Task<Result<StartPullStreamResponse>> StartPullStreamAsync(LiveId liveId, string passphrase, CancellationToken cancellationToken = default) {
        try {
            return await grpcClient.StartPullStreamAsync(
                new StartPullStreamRequest {
                    LiveId     = liveId.ToString(),
                    Passphrase = passphrase
                },
                cancellationToken: cancellationToken
            );
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
        catch (OperationCanceledException) {
            return Errors.Live.LiveOperationCanceled;
        }
    }

    public async Task<Result<StopPullStreamResponse>> StopPullStreamAsync(LiveId liveId, CancellationToken cancellationToken = default) {
        try {
            return await grpcClient.StopPullStreamAsync(
                new StopPullStreamRequest {
                    LiveId = liveId.ToString()
                },
                cancellationToken: cancellationToken
            );
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
        catch (OperationCanceledException) {
            return Errors.Live.LiveOperationCanceled;
        }
    }

    public async Task<Result<string[]>> ListActiveStreamsAsync(CancellationToken cancellationToken = default) {
        try {
            var resp = await grpcClient.ListActiveStreamsAsync(
                new ListActiveStreamsRequest(),
                cancellationToken: cancellationToken
            );
            return resp?.LiveIds?.ToArray() ?? [];
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
        catch (OperationCanceledException) {
            return Errors.Live.LiveOperationCanceled;
        }
    }

    public async Task<Result<GetStreamInfoResponse>> GetStreamStatusAsync(LiveId liveId, CancellationToken cancellationToken = default) {
        try {
            return await grpcClient.GetStreamInfoAsync(
                new GetStreamInfoRequest {
                    LiveId = liveId.ToString()
                },
                cancellationToken: cancellationToken
            );
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
        catch (OperationCanceledException) {
            return Errors.Live.LiveOperationCanceled;
        }
    }

    public Result<IAsyncStreamReader<WatchStreamStatusResponse>> WatchStreamStatusAsync(LiveId liveId, CancellationToken cancellationToken) {
        try {
            var call = grpcClient.WatchStreamStatus(
                new WatchStreamStatusRequest {
                    LiveId = liveId.ToString()
                },
                cancellationToken: cancellationToken
            );
            return new Result<IAsyncStreamReader<WatchStreamStatusResponse>>(call.ResponseStream);
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
        catch (OperationCanceledException) {
            return Errors.Live.LiveOperationCanceled;
        }
    }

}
