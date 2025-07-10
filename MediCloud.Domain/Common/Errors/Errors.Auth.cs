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

    }

}
