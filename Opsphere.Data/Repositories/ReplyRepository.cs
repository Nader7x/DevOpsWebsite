using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class ReplyRepository(ApplicationDbContext dbContext) : BaseRepository<Reply>(dbContext), IReplyRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
}
