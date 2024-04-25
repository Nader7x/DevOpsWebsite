using Opsphere.Data.Models;

namespace Opsphere.Interfaces;

public interface ICardRepository
{
    Task<List<Card>> GetAllAsync();
    Task<Card> CreateAsync(Card cardModel);
}