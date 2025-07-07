namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class User {

        public static Error DuplicateEmail => Error.Conflict(
            "User.DuplicatedEmail",
            "User with given email already exists."
        );

        public static Error InvalidEmailFormat => Error.Conflict(
            "User.InvalidEmailFormat",
            "Format of given email address is invalid."
        );

    }

}
