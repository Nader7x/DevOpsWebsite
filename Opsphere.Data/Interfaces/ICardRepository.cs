using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface ICardRepository  : IBaseRepository<Card>
{
    Task<List<Card>?> GetDeveloperCardsAsync(int devId);
}