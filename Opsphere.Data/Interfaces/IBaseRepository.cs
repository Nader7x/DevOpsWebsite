using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Opsphere.Data.Interfaces;

public interface IBaseRepository <T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<EntityEntry<T>> AddAsync(T entity);
    EntityEntry<T> UpdateAsync(T entity);
    EntityEntry<T> DeleteAsync(T entity);
    Task<T?> FindAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
}