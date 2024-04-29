namespace Opsphere.Dtos.Attachment;

public class AttachmentDto
{
    public int CardId { get; set; }
    public string? FileName { get; set; }
    public byte[]? File { get; set; }
}