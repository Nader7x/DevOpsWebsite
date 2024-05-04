using Opsphere.Data.Models;
using Opsphere.Dtos.Attachment;

namespace Opsphere.Mappers;

public static class AttachmentMapper
{
    public static Attachment ToAttachmentFromCreate(this CreateAttachmentDto attachmentDto, int cardId)
    {
        return new Attachment
        {
            CardId = cardId,
        };
    }

    public static AttachmentDto ToAttachmentDto(this Attachment attachmentModel)
    {
        return new AttachmentDto
        {
            FilePath = attachmentModel.FilePath,
            CardId = attachmentModel.CardId,
            FileUrl = attachmentModel.FileUrl
        };
    }
}