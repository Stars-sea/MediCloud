namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class Live {

        public static Error LiveRoomBanned => Error.Conflict(
            "Live.RoomBanned",
            "Live room is banned."
        );

        public static Error LiveRoomNotFound => Error.Conflict(
            "Live.RoomNotFound",
            "Live room not found."
        );

        public static Error LiveRoomAlreadyExists => Error.Conflict(
            "Live.RoomAlreadyExists",
            "Live room already exists."
        );

        public static Error LiveRoomFailedToDelete => Error.Conflict(
            "LiveRoom.FailedToDelete",
            "Failed to delete live room."
        );

        public static Error LiveRoomFailedToBan => Error.Conflict(
            "LiveRoom.FailedToBan",
            "Failed to ban live room."
        );

        public static Error LiveRoomFailedToUnban => Error.Conflict(
            "LiveRoom.FailedToUnban",
            "Failed to unban live room."
        );

        public static Error LiveRoomFailedToSave => Error.Conflict(
            "LiveRoom.FailedToSave",
            "Failed to save live room."
        );

        public static Error LiveRoomFailedToCreate => Error.Conflict(
            "LiveRoom.FailedToCreate",
            "Failed to create live room."
        );

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
