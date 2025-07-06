namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class User {

        public static Error DuplicateEmail => Error.Conflict(
            "User.DuplicatedEmail",
            "User with given email already exists."
        );

        public static Error RegistrationFailed => Error.Failure(
            "User.RegistrationFailed",
            "Failed to register user."
        );

    }

}
