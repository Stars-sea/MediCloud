namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class User {

        public static Error DuplicateEmail => Error.Conflict(
            "User.DuplicatedEmail",
            "User with given email already exists."
        );

        public static Error UserNotFound => Error.NotFound(
            "User.NotFound",
            "User with given info does not exist."
        );

        public static Error FailedToSave => Error.Conflict(
            "User.FailedToSave",
            "Failed to save user."
        );
        
        public static Error FailedToCreate => Error.Conflict(
            "User.FailedToCreate",
            "Failed to create user."
        );
        
        public static Error FailedToDelete => Error.Conflict(
            "User.FailedToDelete",
            "Failed to delete user."
        );

    }

}
