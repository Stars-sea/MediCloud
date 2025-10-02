namespace MediCloud.Application.Common.Interfaces.Services.Storage;

public interface IImageStorage {

    Task<string> PresignedPutUrlAsync(string name, int expiry, CancellationToken token = default);

    Task<string> PresignedPutUrlAsync(string prefix, string name, int expiry, CancellationToken token = default) {
        return PresignedPutUrlAsync($"{prefix}/{name}", expiry, token);
    }

    Task<string> PresignedGetUrlAsync(string name, int expiry, CancellationToken token = default);

    Task<string> PresignedGetUrlAsync(string prefix, string name, int expiry, CancellationToken token = default) {
        return PresignedGetUrlAsync($"{prefix}/{name}", expiry, token);
    }
    
    Task PutImageAsync(string name, Stream stream, CancellationToken token = default);

    Task PutImageAsync(string prefix, string name, Stream stream, CancellationToken token = default) {
        return PutImageAsync($"{prefix}/{name}", stream, token);
    }

    Task RemoveImageAsync(string name, CancellationToken token = default);

    Task RemoveImageAsync(string prefix, string name, CancellationToken token = default) {
        return RemoveImageAsync($"{prefix}/{name}", token);
    }

    IAsyncEnumerable<string> ListImagesAsync(string prefix, CancellationToken token = default);

}
