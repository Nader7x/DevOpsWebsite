﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Dtos.User;
using Opsphere.Helpers;

namespace Opsphere.Controllers;

[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ApiController]
[Route("Opsphere/User")]
public class UserController(ITokenService tokenService, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please try Again");
        }

        var hashedPassword = UserHandler.HashPassword(registerDto.Password);
        var usernameExists = await _unitOfWork.UserRepository.Checkusername(registerDto.Username);
        if (usernameExists)
        {
            return BadRequest("choose another username");
        }

        var user = new User()
        {
            Email = registerDto.Email,
            Password = hashedPassword,
            Username = registerDto.Username,
            Role = UserRoles.Developer
        };
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();
        return Ok(new UserDto()
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Token = _tokenService.CreateToken(user)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _unitOfWork.UserRepository.Getbyusername(loginDto.Username);
        if (user?.Username != null && (!user.Username.Equals(loginDto.Username, StringComparison.Ordinal)))
            return BadRequest("wrong Credentials");
        var passwordMatch = user?.Password != null && UserHandler.VerifyPassword(loginDto.Password, user.Password);
        if (!passwordMatch)
        {
            return BadRequest("Wrong Credentials");
        }

        return Ok(new UserDto()
        {
            Username = user?.Username,
            Email = user?.Email,
            Token = _tokenService.CreateToken(user)
        });
    }

    [HttpGet("GetCurrentUserName")]
    [Authorize]
    public Task<IActionResult> GetUserName()
    {
        return Task.FromResult<IActionResult>(Ok(User.GetUsername()));
    }


    [HttpGet("/Developers")]
    public async Task<IActionResult> GetAllDevelopers()
    {
        var devs = await _unitOfWork.UserRepository.GetAllAsync(c => c.Role == UserRoles.Developer);
        var devsDtos = _mapper.Map<List<DevDto>>(devs);
        return Ok(devsDtos);
    }

    [HttpGet("UserNotifications/{userid:int}")]
    [Authorize(Roles = "Admin,Developer,TeamLeader")]
    public async Task<IActionResult> GetUserNotifications([FromRoute] int userid)
    {
        var notifications = await _unitOfWork.NotificationRepository.UserNotificationsById(userid);
        var orderedNotifications = notifications.OrderBy(n => n.NotificationDate);
        var filteredNotifications = orderedNotifications.Where(notification => notification.IsRead == false);
        return Ok(filteredNotifications);
    }

    [HttpPatch("AcceptInvite/{userid:int}/Notification/{NotificationId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<IActionResult> Accept([FromRoute] int userid, [FromRoute] int NotificationId)
    {
        var projectDev = await _unitOfWork.ProjectDeveloperRepository.GetByDevIdAsync(userid);
        var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(NotificationId);
        if (projectDev is null or { IsMemeber: true })
        {
            return BadRequest("you already a member or not invited");
        }

        projectDev.IsMemeber = true;
        _unitOfWork.ProjectDeveloperRepository.UpdateAsync(projectDev);
        await _unitOfWork.CompleteAsync();
        notification.IsRead = true;
        _unitOfWork.NotificationRepository.UpdateAsync(notification);
        await _unitOfWork.CompleteAsync();
        return Ok();
    }

    [HttpDelete("Reject/{notificationId:int}")]
    [Authorize(Roles = "Developer,Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject([FromRoute] int notificationId)
    {
        var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
        if (User.GetNameId().IsNullOrEmpty()) return NotFound("maybe your invitation expired");
        var projectDev =
            await _unitOfWork.ProjectDeveloperRepository.GetByDevIdAsync(int.Parse(User.GetNameId() ?? string.Empty));
        if (projectDev is null or { IsTeamLeader: true, IsMemeber: true })
            return NotFound("maybe your invitation expired or has been accepted already");
        _unitOfWork.ProjectDeveloperRepository.DeleteAsync(projectDev);
        await _unitOfWork.CompleteAsync();
        if (notification != null)
        {
            notification.IsRead = true;
            _unitOfWork.NotificationRepository.UpdateAsync(notification);
        }

        await _unitOfWork.CompleteAsync();
        return Ok("Rejected the invitation successfully successfully");
    }
}