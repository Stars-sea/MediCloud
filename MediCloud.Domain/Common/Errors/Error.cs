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
        string code        = "General.Failure",
        string description = "A failure has occurred."
    ) {
        return new Error(code, description, ErrorType.Failure);
    }

    public static Error Unexpected(
        string code        = "General.Unexpected",
        string description = "A unexpected error has occurred."
    ) {
        return new Error(code, description, ErrorType.Unexpected);
    }

    public static Error Validation(
        string code        = "General.Validation",
        string description = "A validation error has occurred."
    ) {
        return new Error(code, description, ErrorType.Validation);
    }

    public static Error Conflict(
        string code        = "General.Conflict",
        string description = "A conflict error has occurred."
    ) {
        return new Error(code, description, ErrorType.Conflict);
    }

    public static Error NotFound(
        string code        = "General.NotFound",
        string description = "A 'Not Found' error has occurred."
    ) {
        return new Error(code, description, ErrorType.NotFound);
    }

    public static Error Unauthorized(
        string code        = "General.Unauthorized",
        string description = "A 'Unauthorized' error has occurred."
    ) {
        return new Error(code, description, ErrorType.Unauthorized);
    }

    public static Error Forbidden(
        string code        = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred."
    ) {
        return new Error(code, description, ErrorType.Forbidden);
    }

}
