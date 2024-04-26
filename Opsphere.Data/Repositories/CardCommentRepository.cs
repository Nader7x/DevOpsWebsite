using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;

public class CardCommentRepository(ApplicationDbContext dbContext) : BaseRepository<CardComment>(dbContext), ICardCommentRepository
{
    
}