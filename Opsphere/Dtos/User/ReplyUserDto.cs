using Opsphere.Data.Models;

namespace Opsphere.Dtos.User;

public class ReplyUserDto
{
    public string? Username { get; set; }
    public string? Email { get; init; }
    public UserRoles Role { get; init; }
}