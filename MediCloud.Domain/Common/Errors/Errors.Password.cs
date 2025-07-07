namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class Password {

        public static Error TooShort => Error.Unexpected(
            "Password.TooShort",
            "Password is too short."
        );

        public static Error RequiresNonAlphanumeric => Error.Unexpected(
            "Password.RequiresNonAlphanumeric",
            "Password requires non-alphanumeric."
        );

        public static Error RequiresDigit => Error.Unexpected(
            "Password.RequiresDigit",
            "Password requires digit."
        );

        public static Error RequiresLower => Error.Unexpected(
            "Password.RequiresLower",
            "Password requires lower."
        );

        public static Error RequiresUpper => Error.Unexpected(
            "Password.RequiresUpper",
            "Password requires upper."
        );
        
    }

}
