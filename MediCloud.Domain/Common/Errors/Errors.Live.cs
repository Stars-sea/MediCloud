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

        public static Error LiveNotFound => Error.NotFound(
            "Live.NotFound",
            "Live not found."
        );

        public static Error LiveFailedToStop => Error.Unexpected(
            "Live.FailedToStop",
            "Failed to stop live stream."
        );

        public static Error LiveFailedToStart => Error.Unexpected(
            "Live.FailedToStart",
            "Failed to start live stream."
        );

        public static Error LiveInternalError => Error.Unexpected(
            "Live.InternalError",
            "An internal error occurred in live service."
        );

        public static Error LiveOperationCanceled => Error.Conflict(
            "Live.OperationCanceled",
            "Operation canceled."
        );

    }

}
