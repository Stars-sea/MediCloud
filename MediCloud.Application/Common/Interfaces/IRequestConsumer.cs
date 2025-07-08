using MassTransit;
using MediCloud.Domain.Common.Contracts;

namespace MediCloud.Application.Common.Interfaces;

public interface IRequestConsumer<in TRequest> : IConsumer<TRequest> where TRequest : class {

    async Task IConsumer<TRequest>.Consume(ConsumeContext<TRequest> context) {
        await context.RespondAsync(await Consume(context));
    }

    public new Task<Result> Consume(ConsumeContext<TRequest> context);

}

public interface IRequestConsumer<in TRequest, TResponse> : IConsumer<TRequest>
    where TRequest : class
    where TResponse : class 
{

    async Task IConsumer<TRequest>.Consume(ConsumeContext<TRequest> context) {
        await context.RespondAsync(await Consume(context));
    }

    public new Task<Result<TResponse>> Consume(ConsumeContext<TRequest> context);

}
