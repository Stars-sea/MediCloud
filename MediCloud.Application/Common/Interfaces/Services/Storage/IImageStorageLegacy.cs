namespace MediCloud.Application.Common.Interfaces.Services.Storage;

public interface IImageStorageLegacy {
    
    IAsyncEnumerable<string> ListImagesAsync(string prefix, CancellationToken cancellationToken = default);

    Task<bool> SaveImageAsync(string prefix, string name, Stream image, CancellationToken cancellationToken = default);

    Task<bool> GetImageAsync(string prefix, string name, Stream output, CancellationToken cancellationToken = default);

    Task<bool> RemoveImageAsync(string prefix, string name, CancellationToken cancellationToken = default);

}
