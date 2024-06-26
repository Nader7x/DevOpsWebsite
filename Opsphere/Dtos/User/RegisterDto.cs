﻿using System.ComponentModel.DataAnnotations;

namespace Opsphere.Dtos.User;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Username { get; set; }
}