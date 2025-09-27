using System.Security.Cryptography;
using System.Text;
using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Common.Interfaces.Services.Storage;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Handlers;

public class AddRecordCommandHandler(
    IImageStorage     imageStorage,
    IRecordRepository recordRepository
) : IRequestHandler<AddRecordCommand, Result<AddRecordCommandResult>> {

    private readonly static HashAlgorithm HashAlgorithm = SHA256.Create();

    // TODO: Extract into a service
    private async static Task<string> ComputeHashAsync(Stream stream, CancellationToken token = default) {
        byte[]        hash = await HashAlgorithm.ComputeHashAsync(stream, token);
        StringBuilder sb   = new StringBuilder();

        foreach (byte b in hash)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    public async Task<Result<AddRecordCommandResult>> Handle(
        AddRecordCommand                 request,
        ConsumeContext<AddRecordCommand> ctx
    ) {
        (UserId userId, Stream imageStream, string remarks) = request;
        string imagePrefix = userId.ToString();
        string imageName   = await ComputeHashAsync(imageStream);

        imageStream.Seek(0, SeekOrigin.Begin);
        if (!await imageStorage.SaveImageAsync(imagePrefix, imageName, imageStream)) {
            await imageStorage.RemoveImageAsync(imagePrefix, imageName);
            return Errors.Record.RecordFailedToSaveImage;
        }

        var    record         = Domain.Record.Record.Factory.Create(userId, imageName, remarks);
        Result dbCreateResult = await recordRepository.CreateRecord(record);

        if (!dbCreateResult.IsSuccess) return dbCreateResult.Errors;
        return new AddRecordCommandResult(record.Id, record.CreatedOn, record.ImageName);
    }

}
