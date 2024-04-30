using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : BaseRepository<Project>(dbContext), IProjectRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<string?> GetProjectNameByIdAsync(int projectId){
        return await _dbContext.Projects.Where(p => p.Id == projectId).Select(p => p.Name).FirstOrDefaultAsync();
    }
}