using FluentValidation;
using MassTransit;
using MediCloud.Application.Common.Contracts;
using MediCloud.Domain.Common.Errors;
using ValidationResult=FluentValidation.Results.ValidationResult;

namespace MediCloud.Application.Common.Validators;

public class ValidationConsumeFilter<TRequest>(
    IValidator<TRequest>? validator
) : IFilter<ConsumeContext<TRequest>> where TRequest : class {

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

        Result errorResult = result.Errors.Select(failure =>
            Error.Validation(failure.ErrorCode, failure.ErrorMessage)
        ).ToArray();
        await context.RespondAsync(errorResult);
    }

    public void Probe(ProbeContext context) { }

}
