namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class Auth {

        public static Error InvalidCred => Error.Validation(
            "Auth.InvalidCred",
            "Invalid credential provided."
        );
        
        public static Error UsernameEmailNotMatch => Error.Unexpected(
            "Auth.UsernameEmailNotMatch",
            "User with given email does not match username."
        );
        
        public static Error PasswordTooShort => Error.Unexpected(
            "Auth.PasswordTooShort",
            "Password is too short."
        );
        
        public static Error PasswordTooLong => Error.Unexpected(
            "Auth.PasswordTooLong",
            "Password is too long."
        );

        public static Error PasswordRequiresNonAlphanumeric => Error.Unexpected(
            "Auth.PasswordRequiresNonAlphanumeric",
            "Password requires non-alphanumeric."
        );

        public static Error PasswordRequiresDigit => Error.Unexpected(
            "Auth.PasswordRequiresDigit",
            "Password requires digit."
        );

        public static Error PasswordRequiresLower => Error.Unexpected(
            "Auth.PasswordRequiresLower",
            "Password requires lower."
        );

        public static Error PasswordRequiresUpper => Error.Unexpected(
            "Auth.PasswordRequiresUpper",
            "Password requires upper."
        );

    }

}
