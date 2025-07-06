namespace MediCloud.Domain.Common.Errors;

public enum ErrorType {

    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden

}

public record Error(
    string    Code,
    string    Description,
    ErrorType Type
) {

    public static Error Failure(
        string code        = "A failure has occurred.",
        string description = "General.Failure"
    ) {
        return new Error(code, description, ErrorType.Failure);
    }

    public static Error Unexpected(
        string code        = "A unexpected error has occurred.",
        string description = "General.Unexpected"
    ) {
        return new Error(code, description, ErrorType.Unexpected);
    }

    public static Error Validation(
        string code        = "A validation error has occurred.",
        string description = "General.Validation"
    ) {
        return new Error(code, description, ErrorType.Validation);
    }

    public static Error Conflict(
        string code        = "A conflict error has occurred.",
        string description = "General.Conflict"
    ) {
        return new Error(code, description, ErrorType.Conflict);
    }

    public static Error NotFound(
        string code        = "A 'Not Found' error has occurred.",
        string description = "General.NotFound"
    ) {
        return new Error(code, description, ErrorType.NotFound);
    }

    public static Error Unauthorized(
        string code        = "A 'Unauthorized' error has occurred.",
        string description = "General.Unauthorized"
    ) {
        return new Error(code, description, ErrorType.Unauthorized);
    }

    public static Error Forbidden(
        string code        = "A 'Forbidden' error has occurred.",
        string description = "General.Forbidden"
    ) {
        return new Error(code, description, ErrorType.Forbidden);
    }

}
