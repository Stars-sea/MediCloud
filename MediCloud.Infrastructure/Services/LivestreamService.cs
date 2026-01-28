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

    public async Task<Result<StartPullStreamResponse>> StartPullStreamAsync(LiveId liveId, string passphrase) {
        try {
            return await grpcClient.StartPullStreamAsync(new StartPullStreamRequest {
                LiveId     = liveId.ToString(),
                Passphrase = passphrase
            });
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
    }

    public async Task<Result<StopPullStreamResponse>> StopPullStreamAsync(LiveId liveId) {
        try {
            return await grpcClient.StopPullStreamAsync(new StopPullStreamRequest {
                LiveId = liveId.ToString()
            });
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
    }

    public async Task<Result<string[]>> ListActiveStreamsAsync() {
        try {
            var resp = await grpcClient.ListActiveStreamsAsync(new ListActiveStreamsRequest());
            return resp?.LiveIds?.ToArray() ?? [];
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
    }

    public async Task<Result<GetStreamStatusResponse>> GetStreamStatusAsync(LiveId liveId) {
        try {
            return await grpcClient.GetStreamStatusAsync(new GetStreamStatusRequest {
                LiveId = liveId.ToString()
            });
        }
        catch (RpcException e) {
            logger.LogError(e, "Failed to interact with grpc: {Message}", e.Message);
            return Errors.Live.LiveInternalError;
        }
    }

}
