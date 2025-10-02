using System.Runtime.CompilerServices;
using MediCloud.Application.Common.Interfaces.Services.Storage;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace MediCloud.Infrastructure.Services.Storage;

public class ImageStorageLegacy(
    IMinioClient                client,
    ILogger<ImageStorageLegacy> logger
) : IImageStorageLegacy {

    public const string BucketName = "medicloud-images-legacy";

    public const string ContentType = "image/jpeg";

    public async Task<bool> EnsureBucketExists(CancellationToken cancellationToken = default) {
        BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
        try {
            if (await client.BucketExistsAsync(bucketExistsArgs, cancellationToken)) return true;
        }
        catch (Exception e) {
            logger.LogError(e, "Failed to check bucket {BucketName}", BucketName);
            return false;
        }

        MakeBucketArgs makeBucketArgs = new MakeBucketArgs().WithBucket(BucketName);
        await client.MakeBucketAsync(makeBucketArgs, cancellationToken);

        return true;
    }

    public async IAsyncEnumerable<string> ListImagesAsync(
        string                                     prefix,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    ) {
        await EnsureBucketExists(cancellationToken);

        ListObjectsArgs listObjectsArgs =
            new ListObjectsArgs()
                .WithBucket(BucketName)
                .WithPrefix(prefix)
                .WithRecursive(true)
                .WithVersions(false);
        await foreach (Item item in client.ListObjectsEnumAsync(listObjectsArgs, cancellationToken))
            yield return item.Key[(prefix.Length + 1)..];
    }

    public async Task<bool> SaveImageAsync(
        string            prefix,
        string            name,
        Stream            image,
        CancellationToken cancellationToken = default
    ) {
        await EnsureBucketExists(cancellationToken);

        PutObjectArgs putObjectArgs =
            new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{prefix}/{name}")
                .WithContentType(ContentType)
                .WithObjectSize(image.Length)
                .WithStreamData(image);
        try { await client.PutObjectAsync(putObjectArgs, cancellationToken); }
        catch (Exception e) {
            logger.LogWarning(e, "Failed to save image");
            return false;
        }

        return true;
    }

    public async Task<bool> GetImageAsync(
        string            prefix,
        string            name,
        Stream            output,
        CancellationToken cancellationToken = default
    ) {
        await EnsureBucketExists(cancellationToken);

        GetObjectArgs getObjectArgs =
            new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{prefix}/{name}")
                .WithCallbackStream(input => input.CopyTo(output));
        try { await client.GetObjectAsync(getObjectArgs, cancellationToken); }
        catch (Exception e) {
            logger.LogWarning(e, "Failed to get image");
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveImageAsync(
        string            prefix,
        string            name,
        CancellationToken cancellationToken = default
    ) {
        await EnsureBucketExists(cancellationToken);

        RemoveObjectArgs removeObjectArgs =
            new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{prefix}/{name}");
        try { await client.RemoveObjectAsync(removeObjectArgs, cancellationToken); }
        catch (Exception e) {
            logger.LogWarning(e, "Failed to remove image");
            return false;
        }

        return true;
    }

}
