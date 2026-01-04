namespace MediCloud.Domain.Common.Errors;

public partial class Errors {

    public class LiveRoom {

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

        public static Error LiveRoomBusy => Error.Conflict(
            "LiveRoom.Busy",
            "Live room is busy."
        );

        public static Error LiveRoomFailedToCreate => Error.Conflict(
            "LiveRoom.FailedToCreate",
            "Failed to create live room."
        );

    }

}
