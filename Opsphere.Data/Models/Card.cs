namespace Opsphere.Data.Models;

public enum Status
{
    Todo,
    InProgress,
    Done
}

public class Card
{
    public int CardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Todo;
    public string Comment { get; set; } = string.Empty;

    public Project? Project { get; set; }
    public int? ProjectId { get; set; }
    
    //The team leader that created this card, he is the only one who can delete it
    public TeamLeader? TeamLeader { get; set; }
    public int? TeamLeaderId { get; set; }
    
    public Developer? Developer { get; set; }
    public int? DeveloperId { get; set; }
    
    
}