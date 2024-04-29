using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Opsphere.Data.Interfaces;

namespace Opsphere.Data.Repositories;

public class BaseRepository<T>(ApplicationDbContext dbContext) : IBaseRepository<T> where T : class
{
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task<EntityEntry<T>> AddAsync(T entity)
    {
        return await dbContext.Set<T>().AddAsync(entity);
    }

    public EntityEntry<T> UpdateAsync(T entity)
    {
          return dbContext.Set<T>().Update(entity);
    }

    public EntityEntry<T> DeleteAsync(T entity)
    {
        return  dbContext.Set<T>().Remove(entity);
    }

    public async Task<T?> FindAsync(T entity)
    {
        return await dbContext.Set<T>().FindAsync(entity);
    }
}