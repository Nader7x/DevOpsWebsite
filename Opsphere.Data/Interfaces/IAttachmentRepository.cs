using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface IAttachmentRepository : IBaseRepository<Attachment>
{
    Task<Attachment?> GetByCardId(int cardId);
}