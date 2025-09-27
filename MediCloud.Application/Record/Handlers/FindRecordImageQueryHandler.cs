using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Services.Storage;
using MediCloud.Application.Record.Contracts;
using MediCloud.Domain.Common.Errors;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Handlers;

public class FindRecordImageQueryHandler(
    IImageStorage imageStorage
) : IRequestHandler<FindRecordImageQuery, Result<Stream>> {

    public async Task<Result<Stream>> Handle(FindRecordImageQuery request, ConsumeContext<FindRecordImageQuery> ctx) {
        (UserId userId, string imageName) = request;

        MemoryStream stream = new();
        if (!await imageStorage.GetImageAsync(userId.ToString(), imageName, stream))
            return Errors.Record.RecordImageNotFound;
        
        stream.Position = 0;
        return stream;
    }

}
