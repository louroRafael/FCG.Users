using FCG.Users.Domain.Common;
using FCG.Users.Domain.DTOs.Requests;
using FCG.Users.Domain.DTOs.Responses;
using FCG.Users.Domain.Errors;
using FCG.Users.Domain.Interfaces.Common;
using FCG.Users.Domain.Interfaces.Repositories;
using FCG.Users.Domain.Interfaces.Security;
using FCG.Users.Domain.Interfaces.Services;

namespace FCG.Users.Services.Auth
{
    public class AuthService(IUserRepository repository, IPasswordHasher passwordHasher, ITokenService tokens, IAppLogger<AuthService> logger) : IAuthService
    {
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            var user = await repository.GetByEmailAsync(request.Email, ct);

            if (user == null)
                return Result<LoginResponse>.Fail(ErrorFactory.Unauthorized("E-mail não registrado"));

            if (!user.IsActive)
                return Result<LoginResponse>.Fail(ErrorFactory.Unauthorized("Usuário inativo"));

            if (!passwordHasher.Verify(user.Password, request.Password))
                return Result<LoginResponse>.Fail(ErrorFactory.Unauthorized("Senha incorreta"));

            var token = tokens.GenerateToken(user);
            var validTo = tokens.GetExpirationDate(token);
            var response = new LoginResponse(token, validTo);

            logger.LogInformation("Login efetuado para o usuário {UserId}.", user.Id);

            return Result<LoginResponse>.Ok(response);
        }
    }
}
