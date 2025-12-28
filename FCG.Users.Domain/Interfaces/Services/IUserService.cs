using FCG.Users.Domain.Common;
using FCG.Users.Domain.DTOs.Requests;
using FCG.Users.Domain.DTOs.Responses;
using FCG.Users.Domain.Enums;

namespace FCG.Users.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserResponse>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct);
        Task<Result<UserResponse>> CreateAdminAsync(CreateAdminRequest request, CancellationToken ct);
        Task<Result<List<UserResponse>>> GetAllUsersAsync(CancellationToken ct);
        Task<Result<UserResponse>> UpdateRoleAsync(Guid userId, ERole newRole, CancellationToken ct);
    }
}
