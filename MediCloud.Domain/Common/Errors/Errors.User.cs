namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class User {

        public static Error DuplicateEmail => Error.Conflict(
            "User.DuplicatedEmail",
            "User with given email already exists."
        );

        public static Error AlreadyHasLiveRoom => Error.Conflict(
            "User.AlreadyHasLiveRoom",
            "User already has a live room."
        );

        public static Error UserNotFound => Error.NotFound(
            "User.NotFound",
            "User with given info does not exist."
        );

        public static Error FailedToUpdate => Error.Conflict(
            "User.FailedToUpdate",
            "Failed to update user."
        );

    }

}
