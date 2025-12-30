using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;
using MediCloud.Domain.User.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record OpenLiveCommand(
    UserId UserId,
    LiveId LiveId
) : Request<Result<OpenLiveCommandResult>>;
