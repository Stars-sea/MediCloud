using Mediator;
using MediCloud.Application.Common.Interfaces.Persistence;
using MediCloud.Application.Record.Contracts;
using MediCloud.Application.Record.Contracts.Mappers;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.Common;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Handlers;

public class AddRecordCommandHandler(
    IRecordRepository recordRepository
) : ICommandHandler<AddRecordCommand, Result<AddRecordCommandResult>> {

    public async ValueTask<Result<AddRecordCommandResult>> Handle(AddRecordCommand command, CancellationToken cancellationToken) {
        (UserId userId, string title, string remarks) = command;

        var    record = Domain.Record.Record.Factory.Create(userId, title, remarks);
        Result result = await recordRepository.CreateRecordAsync(record) & await recordRepository.SaveAsync();

        return result.WithValueIfOk(() => record.MapAddRecordResult());
    }

}
