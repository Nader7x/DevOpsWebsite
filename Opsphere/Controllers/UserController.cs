using Microsoft.AspNetCore.Mvc;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;
using Opsphere.Dtos.User;
using Opsphere.Helpers;
using Opsphere.Services;

namespace Opsphere.Controllers;

[ApiController]
[Route("Opsphere/User")]
public class UserController(ITokenService tokenService, IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var hashedPassword = UserHandler.HashPassword(registerDto.Password);
        var usernameExists = await unitOfWork.UserRepository.Checkusername(registerDto.Username);
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
        await unitOfWork.UserRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();
        return Ok(new UserDto()
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Token = tokenService.CreateToken(user)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await unitOfWork.UserRepository.Getbyusername(loginDto.Username);

        if (user != null && user.Username.Equals(loginDto.Username, StringComparison.Ordinal))
        {
            var passwordMatch = user.Password != null && UserHandler.VerifyPassword(loginDto.Password, user.Password);
            if (!passwordMatch)
            {
                return BadRequest("Wrong Credentials");
            }

            return Ok(new UserDto()
            {
                Username = user.Username,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            });
        }

        return BadRequest("wrong Credentials");
    }

    [HttpGet("GetCurrentUserName")]
    public Task<IActionResult> GetUserName()
    {
        return Task.FromResult<IActionResult>(Ok(User.GetUsername()));
    }

    [HttpPatch("AcceptInvite/{userid:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Accept([FromRoute]int userid)
    {
        var projectDev = await unitOfWork.ProjectDeveloperRepository.GetByDevIdAsync(userid);
        if (projectDev == null || projectDev.IsMemeber == true)
        {
            return BadRequest("User is already a member or not invited");
        }
        projectDev.IsMemeber = true;
        await unitOfWork.CompleteAsync();
        return Ok();
    }
    
}