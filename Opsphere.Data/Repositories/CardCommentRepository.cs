using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;

public class CardCommentRepository(ApplicationDbContext dbContext) : BaseRepository<CardComment>(dbContext), ICardCommentRepository
{
  private readonly ApplicationDbContext _dbContext = dbContext;
  public async Task<List<CardComment>?> GetCardCommentsAsync(int cardId)
  {
      
       return await _dbContext.CardComments.Include(cc => cc.Replies).ThenInclude(r => r.User).Include(cc => cc.User).Where(c => c.CardId == cardId).ToListAsync();
  } 
}