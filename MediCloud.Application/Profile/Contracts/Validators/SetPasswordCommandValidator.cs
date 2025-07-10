using FluentValidation;
using MediCloud.Application.Common.Validators;

namespace MediCloud.Application.Profile.Contracts.Validators;

public sealed class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand> {

    public SetPasswordCommandValidator() {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.OldPassword).Password();
        RuleFor(x => x.NewPassword).Password();
    }

}
