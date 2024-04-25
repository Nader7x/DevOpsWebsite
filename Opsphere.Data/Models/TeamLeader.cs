namespace Opsphere.Data.Models;

public class TeamLeader : User
{
    public List<Project> Projects { get; set; } = [];
    
    public List<Card> Cards { get; set; } = []; //List of cards that this Team Leader created
}