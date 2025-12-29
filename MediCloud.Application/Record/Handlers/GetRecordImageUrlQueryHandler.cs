using MassTransit;
using MediCloud.Application.Common.Interfaces;
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
) : IRequestHandler<GetRecordImageUrlQuery, Result<string>> {

    private int UrlExpirySeconds => minioSettings.Value.UrlExpiryMinutes <= 0
        ? 24 * 60 * 60
        : minioSettings.Value.UrlExpiryMinutes * 60;

    public async Task<Result<string>> Handle(
        GetRecordImageUrlQuery                 request,
        ConsumeContext<GetRecordImageUrlQuery> ctx
    ) {
        (RecordId id, string imageName) = request;

        try { return await imageStorage.PresignedGetUrlAsync(id.ToString(), imageName, UrlExpirySeconds); }
        catch (Exception e) {
            logger.LogWarning(e, "Failed to get image url");
            return Errors.Record.RecordImageNotFound;
        }
    }

}
