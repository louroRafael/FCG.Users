using FCG.Users.Domain.Entities;

namespace FCG.Users.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
        Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct = default);
    }
}
