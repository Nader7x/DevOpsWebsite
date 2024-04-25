namespace Opsphere.Data.Models;

public class Developer : User
{
    public List<Project> Projects { get; set; } = [];
    public List<Card> Cards { get; set; } = [];
}