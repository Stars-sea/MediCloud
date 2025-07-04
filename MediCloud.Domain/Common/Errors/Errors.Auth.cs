using ErrorOr;

namespace MediCloud.Domain.Common.Errors;

public static partial class Errors {
    public static class Auth {
        public static Error InvalidCred => Error.Validation(
            "Auth.InvalidCred",
            "Invalid credential provided."
        );
    }
}
