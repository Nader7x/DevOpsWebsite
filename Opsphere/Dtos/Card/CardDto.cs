namespace Opsphere.Dtos.Card;

public enum Status
{
    Todo,
    InProgress,
    Done
}

public class CardDto
{
    public int CardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Todo;
    public string Comment { get; set; } = string.Empty;
    
    public int? ProjectId { get; set; }
    public int? TeamLeaderId { get; set; }
    public int? DeveloperId { get; set; }
}