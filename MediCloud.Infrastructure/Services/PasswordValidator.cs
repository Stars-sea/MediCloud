using MediCloud.Application.Common.Contracts;
using MediCloud.Application.Common.Interfaces.Services;
using MediCloud.Domain.Common.Errors;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace MediCloud.Infrastructure.Services;

public sealed class PasswordValidator : IPasswordValidator {

    public const int MinLength = 8;
    
    public const int MaxLength = 32;

    public Task<Result> ValidateAsync(string password) {
        List<Error> errors = [];
        
        if (string.IsNullOrWhiteSpace(password) || password.Length < MinLength)
            errors.Add(Errors.Auth.PasswordTooShort);
        
        if (password.Length > MaxLength)
            errors.Add(Errors.Auth.PasswordTooLong);

        if (password.All(IsLetterOrDigit))
            errors.Add(Errors.Auth.PasswordRequiresNonAlphanumeric);
        
        if (!password.Any(IsDigit))
            errors.Add(Errors.Auth.PasswordRequiresDigit);
        
        if (!password.Any(IsLower))
            errors.Add(Errors.Auth.PasswordRequiresLower);
        
        if (!password.Any(IsUpper))
            errors.Add(Errors.Auth.PasswordRequiresUpper);
        
        return Task.FromResult<Result>(errors);
    }

    public static bool IsDigit(char c) { return c is >= '0' and <= '9'; }
    
    public static bool IsLower(char c) { return c is >= 'a' and <= 'z'; }
    
    public static bool IsUpper(char c) { return c is >= 'A' and <= 'Z'; }
    
    public static bool IsLetterOrDigit(char c) { return IsDigit(c) || IsLower(c) || IsUpper(c); }

}
