namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class Live {

        public static Error LiveInvalidStatus => Error.Conflict(
            "Live.InvalidStatus",
            "Live status is invalid."
        );

        public static Error LiveInvalidName => Error.Conflict(
            "Live.InvalidLiveName",
            "Live name is invalid."
        );

        public static Error LiveNotActive => Error.Conflict(
            "Live.NotActive",
            "Live is not active."
        );

        public static Error LiveFailedToSave => Error.Conflict(
            "Live.FailedToSave",
            "Failed to save live."
        );

        public static Error LiveFailedToCreate => Error.Conflict(
            "Live.FailedToCreate",
            "Failed to create live."
        );

        public static Error LiveNotFound => Error.Conflict(
            "Live.NotFound",
            "Live not found."
        );

        public static Error LiveFailedToStop => Error.Conflict(
            "Live.FailedToStop",
            "Failed to stop live stream."
        );

        public static Error LiveFailedToStart => Error.Conflict(
            "Live.FailedToStart",
            "Failed to start live stream."
        );

    }

}
