using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ProjectDeveloperRepository(ApplicationDbContext dbContext) : BaseRepository<ProjectDeveloper>(dbContext), IProjectDeveloperRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<ProjectDeveloper?> GetByDevIdAsync(int devId)
    {
        return await _dbContext.ProjectDevelopers.Include(pd=>pd.User).Where(pd => pd.UserId == devId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectDeveloper>?> GetProjectDevs(int projectId)
    {
        return await _dbContext.ProjectDevelopers
            .Include(pd => pd.User)
            .Include(p=>p.AssignedCards)
            .Where(pd => pd.ProjectId == projectId).ToListAsync();

    }



}