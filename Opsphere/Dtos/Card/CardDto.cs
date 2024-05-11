using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Opsphere.Data.Models;
using Opsphere.Dtos.Project;

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
    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; } = Status.Todo;
    public int? ProjectId { get; set; }
    public int? AssignedDeveloperId { get; set; }
    public CardDevDto ProjectDeveloper { get; set; }
}