using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Opsphere.Data.Models;
using Opsphere.Dtos.Project;
using Opsphere.Interfaces;
using Opsphere.Mappers;

namespace Opsphere.Controllers;
[ApiController]
[Route("Opsphere/project")]

public class ProjectController(IUnitOfWork unitOfWork)  : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var projects = await unitOfWork.ProjectRepository.GetAllAsync();
        var projectsDto = projects.Select(p => p.PrjectToProjectDto());
        return Ok(projectsDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody]CreateProjectDto projectDto)
    {
         var project = projectDto.CreateProjectDtoToProject();
         await unitOfWork.ProjectRepository.AddAsync(project);
         try
         {
             await unitOfWork.CompleteAsync();
         }
         catch (Exception e)
         {
             return BadRequest(e.Message);
         }
         return Created();
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectDto projectDto)
    {
        var project = await unitOfWork.ProjectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        project.Name = projectDto.Name;
        project.Description = projectDto.Description;
        unitOfWork.ProjectRepository.UpdateAsync(project);
        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var project = await unitOfWork.ProjectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        unitOfWork.ProjectRepository.DeleteAsync(project);
        await unitOfWork.CompleteAsync();
        return Ok();
    }

    [HttpPost("{projectId:int}/developer/{developerId:int}")]
    public async Task<IActionResult> AddDeveloper([FromRoute]int projectId,[FromRoute]int developerId)
    {
        var projectDevDto = new AddDevDto()
        {
            ProjectId = projectId,
            UserId = developerId,
            IsTeamLeader = false,
            isMember = false
        };       
        var ProjectName = unitOfWork.ProjectRepository.GetProjectNameByIdAsync(projectId);

        var Notification = new Notification()
        {
            Type = NotificationType.ProjectInvite,
            Content = $"You have been invited to join project {ProjectName} as a developer.",
            userId = developerId,
        };
        var projectDeveloper = projectDevDto.AddDeveloperToProject();
        await unitOfWork.ProjectDeveloperRepository.AddAsync(projectDeveloper);
        await unitOfWork.NotificationRepository.AddAsync(Notification);
        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message + "Maybe the developer is already in the project or doesn't exit");
        }

        return Ok();
    }
}