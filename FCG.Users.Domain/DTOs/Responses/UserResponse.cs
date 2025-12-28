using FCG.Users.Domain.Enums;

namespace FCG.Users.Domain.DTOs.Responses;

public record UserResponse(Guid Id, string Name, string Email, ERole Role = ERole.User);
