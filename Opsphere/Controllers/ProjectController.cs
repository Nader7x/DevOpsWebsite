using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Dtos.Project;
using Opsphere.Helpers;
using Opsphere.Mappers;
using Opsphere.Services;

namespace Opsphere.Controllers;

[ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized),
 ProducesResponseType(StatusCodes.Status500InternalServerError), ApiController, Route("Opsphere/project")]
public class ProjectController(
    IUnitOfWork unitOfWork,
    IHubContext<NotificationService, INotificationService> hubContext,
    NotificationService notificationService) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IHubContext<NotificationService, INotificationService> _hubContext = hubContext;
    private readonly NotificationService _notificationService = notificationService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> GetAll()
    {
        var user = User;
        if (!user.IsInRole("TeamLeader") && !user.IsInRole("Admin")) return Forbid();
        var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
        var projectsDto = projects.Select(p => p.ProjectToProjectDto());
        return Ok(projectsDto);
    }

    [HttpGet("{projectId:int}")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> Get([FromRoute] int projectId)
    {
        var project = await _unitOfWork.ProjectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        return Ok(project.ProjectToProjectDto());
    }

    [HttpGet("WithDevsAndCards/{projectId:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> GetProjectWithDevsAndCards([FromRoute] int projectId)
    {
        var projectQuery = await _unitOfWork.ProjectRepository.ProjectWithDevelopersAsync(projectId);
        var project = projectQuery.FirstOrDefault();
        Console.WriteLine(project);
        if (project != null)
        {
            if (project.ProjectDevelopers != null)
            {
                project.ProjectDevelopers =
                    project.ProjectDevelopers.Where(pd => pd.IsMemeber == true && pd.IsTeamLeader == false).ToList();
                if (project.ProjectDevelopers.Any())
                    return Ok(project.ProjectToProjectDtoWithdevs());
            }

            return Ok(project.ProjectToProjectDto());
        }

        return NotFound();
    }

    [HttpGet("ProjectsOfTeamLeader/{teamleaderId}")]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> GetProjectsOfTeamLeader([FromRoute] int teamleaderId)
    {
        var projects = await unitOfWork.ProjectRepository.GetProjectsOfTeamLeader(teamleaderId);
        var projectsDto = projects?.Select(proj => proj.ProjectToProjectDto());
        return Ok(projectsDto);
    }
    

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto)
    {
        var project = projectDto.CreateProjectDtoToProject();
        var teamLeader = await _unitOfWork.UserRepository.Getbyusername(User.GetUsername());
        project.CreatorId = teamLeader.Id;
        await _unitOfWork.ProjectRepository.AddAsync(project);
        try
        {
            await _unitOfWork.CompleteAsync();
            if (teamLeader != null)
                await _unitOfWork.ProjectDeveloperRepository.AddAsync(new ProjectDeveloper()
                    { ProjectId = project.Id, UserId = teamLeader.Id, IsTeamLeader = true, IsMemeber = true });
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "TeamLeader,Admin")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProjectDto projectDto)
    {
        var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        project.Name = projectDto.Name;
        project.Description = projectDto.Description;
        _unitOfWork.ProjectRepository.UpdateAsync(project);
        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "TeamLeader,Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        _unitOfWork.ProjectRepository.DeleteAsync(project);
        await _unitOfWork.CompleteAsync();
        return Ok();
    }

    [HttpPost("{projectId:int}/developer/{developerId:int}")]
    [Authorize(Roles = "TeamLeader,Admin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddDeveloper([FromRoute] int projectId, [FromRoute] int developerId)
    {
        var projectDevDto = new AddDevDto()
        {
            ProjectId = projectId,
            UserId = developerId,
            IsTeamLeader = false,
            isMember = false
        };
        var projectName = await _unitOfWork.ProjectRepository.GetProjectNameByIdAsync(projectId);

        var notification = new Notification()
        {
            Type = NotificationType.ProjectInvite,
            Content = $"You have been invited to join project {projectName} as a developer.",
            UserId = developerId,
        };
        var projectDeveloper = projectDevDto.AddDeveloperToProject();
        await _unitOfWork.ProjectDeveloperRepository.AddAsync(projectDeveloper);
        try
        {
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest("Maybe the developer is already in the project or doesn't exit");
        }

        foreach (var userConnection in _notificationService.userConnections)
        {
            if (userConnection.Value==developerId.ToString())
            {
                await _hubContext.Clients.Client(userConnection.Key).SendNotification(notification);
            }
        }
        await _hubContext.Clients.Client(_notificationService.userConnections.FirstOrDefault(c=>c.Key==developerId.ToString()).Value).SendNotification(notification);
        return Ok();
    }

    [HttpGet("ProjectDevelopers/{id:int}")]
    public async Task<IActionResult> GetProjectDevs([FromRoute] int id)
    {
        var projectDevs = await _unitOfWork.ProjectDeveloperRepository.GetProjectDevs(id);
        projectDevs = projectDevs.Where(pd => pd.IsTeamLeader == false && pd.IsMemeber == true);
        return Ok(projectDevs.Select(pd => pd.Projectdevtodto()));
    }
}