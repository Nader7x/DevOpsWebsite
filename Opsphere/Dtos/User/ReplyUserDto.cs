using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Opsphere.Data.Models;

namespace Opsphere.Dtos.User;

public class ReplyUserDto
{
    public string? Username { get; set; }
    public string? Email { get; init; }
    [JsonConverter(typeof(StringEnumConverter))]
    public UserRoles Role { get; init; }
}