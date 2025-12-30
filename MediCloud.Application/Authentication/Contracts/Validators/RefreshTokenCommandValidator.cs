using FluentValidation;

namespace MediCloud.Application.Authentication.Contracts.Validators;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand> {

    public RefreshTokenCommandValidator() {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.Jti)
            .NotEmpty().WithMessage("JTI is required");

        RuleFor(x => x.ExpiresStamp)
            .NotEmpty().WithMessage("Expires stamp is required");
    }

}
