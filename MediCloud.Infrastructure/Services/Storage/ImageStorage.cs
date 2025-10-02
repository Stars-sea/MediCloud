using System.Runtime.CompilerServices;
using MediCloud.Application.Common.Interfaces.Services.Storage;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace MediCloud.Infrastructure.Services.Storage;

public class ImageStorage(
    IMinioClient          minio,
    ILogger<ImageStorage> logger
) : IImageStorage {

    private const string BucketName = "medicloud-images";

    private const string ContentType = "image/*";

    // ReSharper disable once UnusedMethodReturnValue.Local
    private async Task<bool> EnsureBucketExists(CancellationToken token = default) {
        BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
        try {
            if (await minio.BucketExistsAsync(bucketExistsArgs, token)) return true;
        }
        catch (MinioException e) {
            logger.LogError(e, "Failed to check bucket {BucketName}", BucketName);
            return false;
        }

        MakeBucketArgs makeBucketArgs = new MakeBucketArgs().WithBucket(BucketName);
        await minio.MakeBucketAsync(makeBucketArgs, token);

        return true;
    }

    public async Task<string> PresignedPutUrlAsync(string name, int expiry, CancellationToken token = default) {
        await EnsureBucketExists(token);

        var args = new PresignedPutObjectArgs()
                   .WithBucket(BucketName)
                   .WithExpiry(expiry)
                   .WithObject(name);
        return await minio.PresignedPutObjectAsync(args);
    }

    public async Task<string> PresignedGetUrlAsync(string name, int expiry, CancellationToken token = default) {
        await EnsureBucketExists(token);

        var args = new PresignedGetObjectArgs()
                   .WithBucket(BucketName)
                   .WithExpiry(expiry)
                   .WithObject(name);
        return await minio.PresignedGetObjectAsync(args);
    }

    public async Task PutImageAsync(string name, Stream stream, CancellationToken token = default) {
        await EnsureBucketExists(token);

        var args = new PutObjectArgs()
                   .WithBucket(BucketName)
                   .WithObject(name)
                   .WithObjectSize(stream.Length)
                   .WithStreamData(stream)
                   .WithContentType(ContentType);
        await minio.PutObjectAsync(args, token);
    }

    public async Task RemoveImageAsync(string name, CancellationToken token = default) {
        await EnsureBucketExists(token);

        var args = new RemoveObjectArgs()
                   .WithBucket(BucketName)
                   .WithObject(name);
        await minio.RemoveObjectAsync(args, token);
    }

    public async IAsyncEnumerable<string> ListImagesAsync(
        string                                     prefix,
        [EnumeratorCancellation] CancellationToken token = default
    ) {
        await EnsureBucketExists(token);

        var args = new ListObjectsArgs()
                   .WithBucket(BucketName)
                   .WithPrefix(prefix)
                   .WithRecursive(true);
        await foreach (var item in minio.ListObjectsEnumAsync(args, token))
            yield return item.Key;
    }

}
