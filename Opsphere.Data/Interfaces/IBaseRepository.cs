using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Opsphere.Interfaces;

public interface IBaseRepository <T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<EntityEntry<T>> AddAsync(T entity);
    EntityEntry<T> UpdateAsync(T entity);
    EntityEntry<T> DeleteAsync(T entity);
    Task<T?> FindAsync(T entity);
}