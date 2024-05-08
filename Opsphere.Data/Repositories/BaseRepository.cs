using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Opsphere.Data.Interfaces;

namespace Opsphere.Data.Repositories;

public class BaseRepository<T>(ApplicationDbContext dbContext) : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<EntityEntry<T>> AddAsync(T entity)
    {
        return await _dbContext.Set<T>().AddAsync(entity);
    }

    public EntityEntry<T> UpdateAsync(T entity)
    {
          return _dbContext.Set<T>().Update(entity);
    }

    public EntityEntry<T> DeleteAsync(T entity)
    {
        return  _dbContext.Set<T>().Remove(entity);
    }

    public async Task<T?> FindAsync(T entity)
    {
        return await _dbContext.Set<T>().FindAsync(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }
}