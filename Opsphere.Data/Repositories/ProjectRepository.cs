using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;

public class ProjectRepository(ApplicationDbContext dbContext) : BaseRepository<Project>(dbContext), IProjectRepository
{
    
}