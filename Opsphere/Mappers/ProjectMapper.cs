using AutoMapper;
using Opsphere.Data.Models;
using Opsphere.Dtos.Project;

namespace Opsphere.Mappers;

public static class ProjectMapper
{
     public static projectDtoWithdevs ProjectToProjectDtoWithdevs(this Project project)
     {
         if (project.ProjectDevelopers != null)
             return new projectDtoWithdevs()
             {
                 Id = project.Id,
                 Name = project.Name,
                 Description = project.Description,
                 ProjectDevelopers = project.ProjectDevelopers.Select(pd => pd.Projectdevtodto())
             };
         return new projectDtoWithdevs() { };
     }
     public static ProjectDto ProjectToProjectDto(this Project project)
     {
         return new ProjectDto
         {
             Id = project.Id,
             Name = project.Name,
             Description = project.Description,
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

     public static ProjectDeveloperDto Projectdevtodto(this ProjectDeveloper projectDeveloper)
     {
         if (projectDeveloper.User == null && projectDeveloper.AssignedCards ==null)
         {
             return new ProjectDeveloperDto();
         }
         if (projectDeveloper.AssignedCards==null)
         {
             return new ProjectDeveloperDto()
             {
                 User = projectDeveloper.User.usertodev(),
             };  
         }

         return new ProjectDeveloperDto()
         {
             User = projectDeveloper.User.usertodev(),
             AssignedCards = projectDeveloper.AssignedCards.Select(c => c.ToCardDto())
         };
     }
}