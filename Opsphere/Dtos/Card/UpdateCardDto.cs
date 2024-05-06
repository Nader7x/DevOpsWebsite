using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Opsphere.Dtos.Card;


public class UpdateCardDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; }
}