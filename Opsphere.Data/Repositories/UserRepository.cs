using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public Task<bool> Checkusername(string username)
    {
        return dbContext.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> Getbyusername(string username)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}