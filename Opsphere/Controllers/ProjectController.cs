using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Dtos.Project;
using Opsphere.Helpers;
using Opsphere.Mappers;

namespace Opsphere.Controllers;

[ApiController]
[Route("Opsphere/project")]
public class ProjectController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var user = User;
        if (user.IsInRole("TeamLeader") || user.IsInRole("Admin"))
        {
            var projects = await unitOfWork.ProjectRepository.GetAllAsync();
            var projectsDto = projects.Select(p => p.PrjectToProjectDto());
            return Ok(projectsDto);
        }

        return Forbid();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto)
    {
        var project = projectDto.CreateProjectDtoToProject();
        var teamLeader = await unitOfWork.UserRepository.Getbyusername(User.GetUsername());
        await unitOfWork.ProjectRepository.AddAsync(project);
        try
        {
            await unitOfWork.CompleteAsync();
            if (teamLeader != null)
                await unitOfWork.ProjectDeveloperRepository.AddAsync(new ProjectDeveloper()
                    {ProjectId = project.Id ,UserId = teamLeader.Id, IsTeamLeader = true, IsMemeber = true });
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "TeamLeader,Admin")]
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
    [Authorize(Roles = "TeamLeader,Admin")]
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
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> AddDeveloper([FromRoute] int projectId, [FromRoute] int developerId)
    {
        var projectDevDto = new AddDevDto()
        {
            ProjectId = projectId,
            UserId = developerId,
            IsTeamLeader = false,
            isMember = false
        };
        var projectName = await unitOfWork.ProjectRepository.GetProjectNameByIdAsync(projectId);

        var notification = new Notification()
        {
            Type = NotificationType.ProjectInvite,
            Content = $"You have been invited to join project {projectName} as a developer.",
            UserId = developerId,
        };
        var projectDeveloper = projectDevDto.AddDeveloperToProject();
        await unitOfWork.ProjectDeveloperRepository.AddAsync(projectDeveloper);
        try
        {
            await unitOfWork.CompleteAsync();
            await unitOfWork.NotificationRepository.AddAsync(notification);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message + "Maybe the developer is already in the project or doesn't exit");
        }

        return Ok();
    }
}