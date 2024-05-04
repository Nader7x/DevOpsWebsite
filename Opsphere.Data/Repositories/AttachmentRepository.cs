using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class AttachmentRepository(ApplicationDbContext dbContext) : BaseRepository<Attachment>(dbContext), IAttachmentRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Attachment?> GetByCardId(int cardId)
    {
       return await _dbContext.Attachments.FirstOrDefaultAsync(a => a != null && a.CardId == cardId);
    }
}