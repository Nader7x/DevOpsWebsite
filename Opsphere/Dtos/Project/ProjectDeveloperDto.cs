using Opsphere.Dtos.Card;
using Opsphere.Dtos.User;

namespace Opsphere.Dtos.Project;

public class ProjectDeveloperDto
{
    public DevDto? User { get; set; }
    public IEnumerable<CardDto>? AssignedCards { get; set; }
}