using FCG.Users.Domain.Common;
using FCG.Users.Domain.DTOs.Requests;
using FCG.Users.Domain.DTOs.Responses;

namespace FCG.Users.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
    }
}
