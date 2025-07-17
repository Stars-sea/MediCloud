using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public record OpenLiveCommand(
    string LiveName,
    UserId UserId
) : Request<Result<OpenLiveCommandResult>>;
