using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces;
using MediCloud.Application.Live.Contracts;
using MediCloud.Application.Live.Contracts.Results;

namespace MediCloud.Application.Live.Handlers;

public class GetLiveStatusQueryHandler : IRequestHandler<GetLiveStatusQuery, Result<GetLiveStatusQueryResult>> {

    public Task<Result<GetLiveStatusQueryResult>> Handle(
        GetLiveStatusQuery                 request,
        ConsumeContext<GetLiveStatusQuery> ctx
    ) {
        throw new NotImplementedException();
    }

}
