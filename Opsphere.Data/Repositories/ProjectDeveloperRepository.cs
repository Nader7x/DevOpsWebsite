using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ProjectDeveloperRepository(ApplicationDbContext dbContext) : BaseRepository<ProjectDeveloper>(dbContext), IProjectDeveloperRepository
{
    
}