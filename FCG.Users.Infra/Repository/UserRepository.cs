using FCG.Users.Domain.Entities;
using FCG.Users.Domain.Interfaces.Repositories;
using FCG.Users.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Users.Infra.Repository
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            return _dbSet.AnyAsync(u => u.Email == email, ct);
        }

        public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);
        }
    }
}
