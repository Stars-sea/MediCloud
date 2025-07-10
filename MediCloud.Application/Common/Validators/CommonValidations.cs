using FluentValidation;

namespace MediCloud.Application.Common.Validators;

public static class CommonValidations {
    
    private const string SpecialChars = "!@#$%^&*,.?-+_=;:";

    public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return ruleBuilder
               .NotEmpty().WithMessage("Username is required")
               .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
               .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
               .Must(BeAllAlphanumericAndLine).WithMessage("Username must contain only \"-\", \"_\" and alphanumeric characters");
    }

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder) {
        return ruleBuilder
               .NotEmpty().WithMessage("Password is required")
               .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
               .MaximumLength(32).WithMessage("Password must not exceed 32 characters")
               .Must(ContainsDigit).WithMessage("Password must contain digits")
               .Must(ContainsLower).WithMessage("Password must contain lowercase letters")
               .Must(ContainsUpper).WithMessage("Password must contain uppercase letters")
               .Must(ContainsSpecialChar).WithMessage($"Password must contain special characters (any of \"{SpecialChars}\")");
    }

    private static bool ContainsDigit(string s) { return s.Any(c => c is >= '0' and <= '9'); }

    private static bool ContainsLower(string s) { return s.Any(c => c is >= 'a' and <= 'z'); }

    private static bool ContainsUpper(string s) { return s.Any(c => c is >= 'A' and <= 'Z'); }

    private static bool ContainsSpecialChar(string s) { return s.Any(c => SpecialChars.Contains(c)); }

    private static bool BeAllAlphanumericAndLine(string s) {
        return s.All(c => c is
            >= 'A' and <= 'Z' or
            >= 'a' and <= 'z' or
            >= '0' and <= '9' or
            '-' or '_'
        );
    }
}
