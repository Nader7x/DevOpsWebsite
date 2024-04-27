using Opsphere.Data.Models;

namespace Opsphere.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<string> GetProjectNameByIdAsync(int projectId);
}