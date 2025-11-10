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

        public static Error LiveRoomFailedToUpdate => Error.Conflict(
            "LiveRoom.FailedToUpdate",
            "Failed to update live room."
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

    }

}
