using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;
using MediCloud.Domain.Live.ValueObjects;

namespace MediCloud.Application.Live.Contracts;

public sealed record GetLiveStatusQuery(
    LiveId LiveId
) : Request<Result<GetLiveStatusQueryResult>>;
