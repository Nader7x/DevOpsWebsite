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
}