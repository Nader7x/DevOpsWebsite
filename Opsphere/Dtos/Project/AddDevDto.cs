namespace Opsphere.Dtos.Project;

public class AddDevDto
{
    public int ProjectId { get; set; }
    public int UserId { get; set; }
    public bool IsTeamLeader { get; set; } = false;
}