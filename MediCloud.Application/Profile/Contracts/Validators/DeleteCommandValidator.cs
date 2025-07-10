using FluentValidation;
using MediCloud.Application.Common.Validators;

namespace MediCloud.Application.Profile.Contracts.Validators;

public sealed class DeleteCommandValidator : AbstractValidator<DeleteCommand> {

    public DeleteCommandValidator() {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.Username).Username();
        
        RuleFor(x => x.Password).Password();
    }

}
