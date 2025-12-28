using FCG.Users.Domain.Entities;

namespace FCG.Users.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task AddAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(T entity, CancellationToken ct = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        Task<T> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        
    }
}
