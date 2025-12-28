using FCG.Users.Domain.Entities;
using FCG.Users.Domain.Interfaces.Repositories;
using FCG.Users.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Users.Infra.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
