using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Record.Contracts.Result;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Record.Contracts;

public record AddRecordCommand(
    UserId   UserId,
    Stream   Image,
    string   Remarks = ""
) : Request<Result<AddRecordCommandResult>>;
