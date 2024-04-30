using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ProjectDeveloperRepository(ApplicationDbContext dbContext) : BaseRepository<ProjectDeveloper>(dbContext), IProjectDeveloperRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<ProjectDeveloper?> GetByDevIdAsync(int devId)
    {
        return await _dbContext.ProjectDevelopers.Where(pd => pd.UserId == devId).FirstOrDefaultAsync();
    }


}