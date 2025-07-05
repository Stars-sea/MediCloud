namespace MediCloud.Domain.Common.Errors;

public enum ErrorType {
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
}

public record Error(
    string    Message,
    string    Code,
    ErrorType Type
) {
    public static Error Failure(
        string message = "General.Failure",
        string code    = "A failure has occurred."
    ) => new(message, code, ErrorType.Failure);

    public static Error Unexpected(
        string message = "General.Unexpected",
        string code    = "A unexpected error has occurred."
    ) => new(message, code, ErrorType.Unexpected);

    public static Error Validation(
        string message = "General.Validation",
        string code    = "A validation error has occurred."
    ) => new(message, code, ErrorType.Validation);

    public static Error Conflict(
        string message = "General.Conflict",
        string code    = "A conflict error has occurred."
    ) => new(message, code, ErrorType.Conflict);

    public static Error NotFound(
        string message = "General.NotFound",
        string code    = "A 'Not Found' error has occurred."
    ) => new(message, code, ErrorType.NotFound);

    public static Error Unauthorized(
        string message = "General.Unauthorized",
        string code    = "A 'Unauthorized' error has occurred."
    ) => new(message, code, ErrorType.Unauthorized);

    public static Error Forbidden(
        string message = "General.Forbidden",
        string code    = "A 'Forbidden' error has occurred."
    ) => new(message, code, ErrorType.Forbidden);
}
