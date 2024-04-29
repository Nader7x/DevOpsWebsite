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
        if (user == null)
        {
            return BadRequest("Wrong Credentials");
        }
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
}