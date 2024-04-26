using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;

public class AttachmentRepository(ApplicationDbContext dbContext) : BaseRepository<Attachment>(dbContext), IAttachmentRepository
{
    
}