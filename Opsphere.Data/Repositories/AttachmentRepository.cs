using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class AttachmentRepository(ApplicationDbContext dbContext) : BaseRepository<Attachment>(dbContext), IAttachmentRepository
{
    
}