using FCG.Users.Domain.Enums;

public record Error(
    ErrorType Type,
    string Code,
    string Message
);
