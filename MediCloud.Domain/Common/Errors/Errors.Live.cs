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

    }

}
