using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : BaseRepository<Project>(dbContext), IProjectRepository
{

    public async Task<string> GetProjectNameByIdAsync(int projectId){
        return await dbContext.Projects.Where(p => p.Id == projectId).Select(p => p.Name).FirstOrDefaultAsync();
    }
}