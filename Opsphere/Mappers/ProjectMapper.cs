using Opsphere.Data.Models;
using Opsphere.Dtos.Project;

namespace Opsphere.Mappers;

public static class ProjectMapper
{
     public static ProjectDto PrjectToProjectDto(this Project project)
     {
         return new ProjectDto
         {
             Id = project.Id,
             Name = project.Name,
             Description = project.Description
         };
     }
     public static Project CreateProjectDtoToProject(this CreateProjectDto createProjectDto)
     {
         return new Project
         {
             Name = createProjectDto.Name,
             Description = createProjectDto.Description
         };
     }
     public static ProjectDeveloper AddDeveloperToProject(this AddDevDto devDto)
     {
         return new ProjectDeveloper
         {
             ProjectId = devDto.ProjectId,
             UserId = devDto.UserId,
             IsTeamLeader = devDto.IsTeamLeader
         };
     }
}