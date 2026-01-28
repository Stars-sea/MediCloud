using MediCloud.Application.Common.Protos;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Common.Interfaces.Services;

public interface ILivestreamService {

    Task<Result<StartPullStreamResponse>> StartPullStreamAsync(LiveId liveId, string passphrase);
    
    Task<Result<StopPullStreamResponse>> StopPullStreamAsync(LiveId liveId);
    
    Task<Result<string[]>> ListActiveStreamsAsync();
    
    Task<Result<GetStreamStatusResponse>> GetStreamStatusAsync(LiveId liveId);

}
