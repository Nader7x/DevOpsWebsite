using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface ICardCommentRepository : IBaseRepository<CardComment>
{
    Task<List<CardComment>?> GetCardCommentsAsync(int cardId);
}