using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Live.Contracts.Results;

namespace MediCloud.Application.Live.Contracts;

public record GetLiveStatusQuery(
    string LiveId
) : Request<Result<GetLiveStatusQueryResult>>;
