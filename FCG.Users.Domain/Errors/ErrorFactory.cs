using FCG.Users.Domain.Enums;

namespace FCG.Users.Domain.Errors
{
    public static class ErrorFactory
    {
        public static Error Validation(string message, string code = "VALIDATION_ERROR")
        => new(ErrorType.Validation, code, message);

        public static Error Unauthorized(string message = "Invalid credentials", string code = "UNAUTHORIZED")
            => new(ErrorType.Unauthorized, code, message);

        public static Error NotFound(string message, string code = "NOT_FOUND")
            => new(ErrorType.NotFound, code, message);

        public static Error Conflict(string message, string code = "CONFLICT")
            => new(ErrorType.Conflict, code, message);
    }
}
