using Opsphere.Data.Models;
using Opsphere.Dtos.User;

namespace Opsphere.Mappers;

public static class UserMapper
{
    public static DevDto usertodev(this User user)
    {
        return new DevDto()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }

    public static User UserToUserNameAndEmail(this User user)
    {
        return new User
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public static ReplyUserDto ToReplyUserDtoFromUser(this User user)
    {
        return new ReplyUserDto
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }
}