using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class CardCommentRepository(ApplicationDbContext dbContext) : BaseRepository<CardComment>(dbContext), ICardCommentRepository
{
    
}