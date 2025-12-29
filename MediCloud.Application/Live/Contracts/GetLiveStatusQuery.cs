using MassTransit.Mediator;
using MediCloud.Application.Live.Contracts.Results;
using MediCloud.Domain.Common;

namespace MediCloud.Application.Live.Contracts;

public record GetLiveStatusQuery(
    string LiveId
) : Request<Result<GetLiveStatusQueryResult>>;
