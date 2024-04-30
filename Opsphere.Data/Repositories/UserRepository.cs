using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<bool> Checkusername(string username)
    {
        return _dbContext.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> Getbyusername(string? username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u != null && u.Username == username);
    }
}