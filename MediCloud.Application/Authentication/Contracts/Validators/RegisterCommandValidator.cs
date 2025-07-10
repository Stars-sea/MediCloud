using FluentValidation;
using MediCloud.Application.Common.Validators;

namespace MediCloud.Application.Authentication.Contracts.Validators;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand> {

    public RegisterCommandValidator() {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.Password).Password();

        RuleFor(x => x.Username).Username();
    }

}
