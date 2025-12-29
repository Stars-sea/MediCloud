using MassTransit;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Handlers;

public class AddRecordCommandHandler(
    IRecordRepository recordRepository
) : IRequestHandler<AddRecordCommand, Result<AddRecordCommandResult>> {

    public async Task<Result<AddRecordCommandResult>> Handle(
        AddRecordCommand                 request,
        ConsumeContext<AddRecordCommand> ctx
    ) {
        (UserId userId, string title, string remarks) = request;

        var    record         = Domain.Record.Record.Factory.Create(userId, title, remarks);
        Result dbCreateResult = await recordRepository.CreateRecordAsync(record);

        if (!dbCreateResult.IsSuccess) return dbCreateResult.Errors;
        return new AddRecordCommandResult(record.Id, record.CreatedOn);
    }

}
