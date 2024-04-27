using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : BaseRepository<Project>(dbContext), IProjectRepository
{

    public Task<string> GetProjectNameByIdAsync(int projectId){
        return dbContext.Projects.Where(p => p.Id == projectId).Select(p => p.Name).FirstOrDefaultAsync();
    }
}