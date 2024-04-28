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
            FileName = attachmentDto.FileName,
            File = attachmentDto.File,
            
        };
    }

    public static AttachmentDto ToAttachmentDto(this Attachment attachmentModel)
    {
        return new AttachmentDto
        {
            File = attachmentModel.File,
            FileName = attachmentModel.FileName,
            CardId = attachmentModel.CardId
        };
    }
}