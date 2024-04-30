using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface IProjectDeveloperRepository  : IBaseRepository<ProjectDeveloper>
{
    Task<ProjectDeveloper?> GetByDevIdAsync(int devId);
}