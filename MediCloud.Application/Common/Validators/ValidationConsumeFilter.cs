using System.Reflection;
using FluentValidation;
using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Common.Errors;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MediCloud.Application.Common.Validators;

public class ValidationConsumeFilter<TRequest>(
    IValidator<TRequest>? validator = null
) : IFilter<ConsumeContext<TRequest>> where TRequest : class //, Request<TResponse>
{

    public async Task Send(ConsumeContext<TRequest> context, IPipe<ConsumeContext<TRequest>> next) {
        if (validator is null) {
            await next.Send(context);
            return;
        }

        ValidationResult result = await validator.ValidateAsync(context.Message);
        if (result.IsValid) {
            await next.Send(context);
            return;
        }

        await context.RespondAsync(MapValidationResult(result));
        await context.NotifyConsumed(context, TimeSpan.Zero, "ConsumeFilter");
    }

    public void Probe(ProbeContext context) { context.CreateFilterScope("ConsumeFilter"); }

    private static object MapValidationResult(ValidationResult result) {
        Type targetType = typeof(TRequest)
                          .GetInterface(typeof(MassTransit.Mediator.Request<>).Name)!
                          .GetGenericArguments()[0];

        Result errorResult = result.Errors.Select(failure =>
            Error.Validation(failure.ErrorCode, failure.ErrorMessage)
        ).ToArray();

        if (targetType == errorResult.GetType())
            return errorResult;

        ConstructorInfo info = targetType.GetConstructor([errorResult.GetType()])!;
        return info.Invoke([errorResult]);
    }

}
