using FluentValidation;
using MediCloud.Application.Common.Validators;

namespace MediCloud.Application.Profile.Contracts.Validators;

public sealed class FindByNameQueryValidator : AbstractValidator<FindUserByNameQuery> {

    public FindByNameQueryValidator() {
        RuleFor(x => x.UserName).Username();
    }

}
