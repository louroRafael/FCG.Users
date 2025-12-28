using FCG.Users.Domain.Common;
using FCG.Users.Domain.DTOs.Requests;
using FCG.Users.Domain.DTOs.Responses;
using FCG.Users.Domain.Entities;
using FCG.Users.Domain.Enums;
using FCG.Users.Domain.Errors;
using FCG.Users.Domain.Interfaces.Repositories;
using FCG.Users.Domain.Interfaces.Security;
using FCG.Users.Domain.Interfaces.Services;

namespace FCG.Users.Services.User
{
    public class UserService(IUserRepository repository, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct)
        {
            return await CreateInternalAsync(request.Name, request.Email, request.Password, ERole.User, ct);
        }

        public async Task<Result<UserResponse>> CreateAdminAsync(CreateAdminRequest request, CancellationToken ct)
        {
            return await CreateInternalAsync(request.Name, request.Email, request.Password, request.Role, ct);
        }

        public async Task<Result<List<UserResponse>>> GetAllUsersAsync(CancellationToken ct)
        {
            var users = await repository.GetAllAsync(ct);

            return Result<List<UserResponse>>.Ok(
                users.Select(x => new UserResponse(
                    x.Id,
                    x.Name,
                    x.Email,
                    x.Role
                )).ToList()
            );
        }

        public async Task<Result<UserResponse>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await repository.GetByIdAsync(id, ct);

            return user is null
                ? Result<UserResponse>.Fail(ErrorFactory.NotFound("Usuário não encontrado."))
                : Result<UserResponse>.Ok(new UserResponse(user.Id, user.Name, user.Email, user.Role));
        }

        public async Task<Result<UserResponse>> UpdateRoleAsync(Guid userId, ERole newRole, CancellationToken ct)
        {
            var user = await repository.GetByIdAsync(userId, ct); 

            if (user == null)
                return Result<UserResponse>.Fail(ErrorFactory.NotFound("Usuário não encontrado."));

            user.UpdateRole(newRole);

            await repository.UpdateAsync(user, ct);
            return Result<UserResponse>.Ok(new UserResponse(user.Id, user.Name, user.Email, user.Role));
        }

        private async Task<Result<UserResponse>> CreateInternalAsync(string name, string email, string password, ERole role, CancellationToken ct)
        {
            if (await repository.EmailExistsAsync(email, ct))
                return Result<UserResponse>.Fail(ErrorFactory.Validation("E-mail já cadastrado."));

            var hashPassword = passwordHasher.Hash(password);
            var user = new UserEntity(name, email, hashPassword, role);

            await repository.AddAsync(user, ct);

            return Result<UserResponse>.Ok(new UserResponse(user.Id, user.Name, user.Email, user.Role));
        }
    }
}
