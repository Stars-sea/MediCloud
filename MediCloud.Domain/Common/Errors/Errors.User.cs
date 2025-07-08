namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class User {

        public static Error DuplicateEmail => Error.Conflict(
            "User.DuplicatedEmail",
            "User with given email already exists."
        );

        public static Error InvalidEmail => Error.Unexpected(
            "User.InvalidEmail",
            "Email address is not valid."
        );

        public static Error InvalidUsername => Error.Unexpected(
            "User.InvalidUsername",
            "Username is not valid."
        );

        public static Error UsernameEmailNotMatch => Error.Unexpected(
            "User.UsernameEmailNotMatch",
            "User with given email does not match username."
        );

        public static Error FailedToUpdate => Error.Conflict(
            "User.FailedToUpdate",
            "Failed to update user."
        );

    }

}
