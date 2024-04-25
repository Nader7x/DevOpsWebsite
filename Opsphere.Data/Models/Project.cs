namespace Opsphere.Data.Models;

public class Project
{
    public int ProjectId { get; set; }
    
    public TeamLeader? TeamLeader { get; set; }
    public int? TeamLeaderId { get; set; }
    
    public List<Card> Cards { get; set; } = [];
    public List<Developer> Developers { get; set; } = [];
}