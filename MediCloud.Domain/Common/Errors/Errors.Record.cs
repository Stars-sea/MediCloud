namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {

    public static class Record {

        public static Error RecordFailedToCreate => Error.Conflict(
            "Record.FailedToCreate",
            "Failed to create record"
        );

        public static Error RecordFailedToDelete => Error.Conflict(
            "Record.FailedToDelete",
            "Failed to delete record"
        );

        public static Error RecordNotFound => Error.NotFound(
            "Record.NotFound",
            "Record not found"
        );

        public static Error RecordFailedToSaveImage => Error.Failure(
            "Record.FailedToSaveImage",
            "Failed to save image"
        );

        public static Error RecordInvalidImageSize => Error.Conflict(
            "Record.InvalidImageSize",
            "Invalid image size"
        );
        
        public static Error RecordImageNotFound => Error.NotFound(
            "Record.ImageNotFound",
            "Image not found"
        );

    }

}
