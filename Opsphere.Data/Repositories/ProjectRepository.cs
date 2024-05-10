using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : BaseRepository<Project>(dbContext), IProjectRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<string?> GetProjectNameByIdAsync(int projectId)
    {
        return await _dbContext.Projects.Where(p => p.Id == projectId).Select(p => p.Name).FirstOrDefaultAsync();
    }

    public async Task<IQueryable<Project>> ProjectWithDevelopersAsync(int projectId)
    {
        return _dbContext.Projects
            .Include(x => x.ProjectDevelopers)!
            .ThenInclude(x => x.User)
            .Include(p=>p.ProjectDevelopers)
            .ThenInclude(pd => pd.AssignedCards)
            .Where(p => p.Id == projectId);
    }

    public async Task<List<Project>?> GetProjectsOfTeamLeader(int teamleaderId)
    {
        var projects = await _dbContext.Projects.Where(proj => proj.CreatorId == teamleaderId).ToListAsync();
        
        return projects;
    }
}