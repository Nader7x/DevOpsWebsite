﻿using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<string?> GetProjectNameByIdAsync(int projectId);
    Task<IQueryable<Project>> ProjectWithDevelopersAsync(int projectId);

    Task<List<Project>?> GetProjectsOfTeamLeader(int teamleaderId);
}