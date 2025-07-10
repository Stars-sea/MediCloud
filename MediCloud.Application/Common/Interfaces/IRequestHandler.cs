using MassTransit;
using MassTransit.Mediator;
using MediCloud.Application.Common.Contracts;

namespace MediCloud.Application.Common.Interfaces;

public interface IRequestHandler<in TRequest, TResponse> : IConsumer<TRequest>
    where TRequest : class, Request<TResponse>
    where TResponse : Result {

    async Task IConsumer<TRequest>.Consume(ConsumeContext<TRequest> context) {
        await context.RespondAsync(await Handle(context.Message));
    }

    public Task<TResponse> Handle(TRequest request);

}

public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : class, Request<Result>;
