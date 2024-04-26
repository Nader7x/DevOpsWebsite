using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;

public class ProjectDeveloperRepository(ApplicationDbContext dbContext) : BaseRepository<ProjectDeveloper>(dbContext), IProjectDeveloperRepository
{
    
}