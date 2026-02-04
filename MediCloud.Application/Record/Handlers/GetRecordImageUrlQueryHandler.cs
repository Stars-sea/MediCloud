using Mediator;
using MediCloud.Application.Common.Interfaces.Services.Storage;
using MediCloud.Application.Common.Settings;
using MediCloud.Application.Record.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Record.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediCloud.Application.Record.Handlers;

public class GetRecordImageUrlQueryHandler(
    IImageStorage                          imageStorage,
    IOptions<MinioSettings>                minioSettings,
    ILogger<GetRecordImageUrlQueryHandler> logger
) : IQueryHandler<GetRecordImageUrlQuery, Result<string>> {

    private int UrlExpirySeconds => minioSettings.Value.UrlExpiryMinutes <= 0
        ? 24 * 60 * 60
        : minioSettings.Value.UrlExpiryMinutes * 60;

    public async ValueTask<Result<string>> Handle(GetRecordImageUrlQuery query, CancellationToken cancellationToken) {
        (RecordId id, string imageName) = query;

        try { return await imageStorage.PresignedGetUrlAsync(id.ToString(), imageName, UrlExpirySeconds, cancellationToken); }
        catch (Exception e) {
            logger.LogWarning(e, "Failed to get image url");
            return Errors.Record.RecordImageNotFound;
        }
    }

}
