using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services.Storage;
using MediCloud.Application.Record.Contracts;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.Record.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MediCloud.Application.Record.Handlers;

public class AddRecordImageCommandHandler(
    IImageStorage                         imageStorage,
    IRecordRepository                     recordRepository,
    ILogger<AddRecordImageCommandHandler> logger
) : IRequestHandler<AddRecordImageCommand, Result<string>> {

    private async Task RemoveImageSilentlyAsync(string prefix, string name, CancellationToken token = default) {
        try { await imageStorage.RemoveImageAsync(prefix, name, token); }
        catch (Exception) { logger.LogDebug("Removing image {Prefix}/{Name} failed", prefix, name); }
    }

    public async Task<Result<string>> Handle(
        AddRecordImageCommand                 request,
        ConsumeContext<AddRecordImageCommand> ctx
    ) {
        (RecordId id, Stream stream) = request;

        Domain.Record.Record? record = await recordRepository.FindRecordByIdAsync(id);
        if (record is null) return Errors.Record.RecordNotFound;

        string prefix = record.Id.ToString();
        string name   = Guid.NewGuid().ToString();

        try { await imageStorage.PutImageAsync(prefix, name, stream); }
        catch (Exception e) {
            logger.LogWarning(e, "Failed to save image");
            await RemoveImageSilentlyAsync(prefix, name);

            return Errors.Record.RecordFailedToSaveImage;
        }

        var result = await recordRepository.AddRecordImageAsync(record, name) & await recordRepository.SaveAsync();
        if (result.IsSuccess) return name;

        logger.LogWarning("Failed to add image {Name} in database", name);
        await RemoveImageSilentlyAsync(prefix, name);

        return Errors.Record.RecordFailedToSaveImage;
    }

}
