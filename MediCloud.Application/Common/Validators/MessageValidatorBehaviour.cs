using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using Mediator;
using MediCloud.Domain.Common;
using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Common.Validators;

public sealed class MessageValidatorBehaviour<TMessage, TResponse>(
    IValidator<TMessage>? validator = null
) : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result {

    public async ValueTask<TResponse> Handle(
        TMessage                                    message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken                           cancellationToken
    ) {
        if (validator is null) {
            return await next(message, cancellationToken);
        }

        ValidationResult result = await validator.ValidateAsync(message, cancellationToken);
        if (result.IsValid) {
            return await next(message, cancellationToken);
        }

        return (TResponse)MapValidationResult(result);
    }

    private static object MapValidationResult(ValidationResult result) {
        string[] interfaceNames = [typeof(IQuery<>).Name, typeof(ICommand<>).Name, typeof(IRequest<>).Name];
        Type     targetType     = interfaceNames.Select(n => typeof(TMessage).GetInterface(n)).First(t => t is not null)!;

        Result errorResult = result.Errors.Select(failure =>
            Error.Validation(failure.ErrorCode, failure.ErrorMessage)
        ).ToArray();

        if (targetType == errorResult.GetType())
            return errorResult;

        ConstructorInfo info = targetType.GetConstructor([errorResult.GetType()])!;
        return info.Invoke([errorResult]);
    }

}
