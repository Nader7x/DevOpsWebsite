using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;


public class CardRepository(ApplicationDbContext dbContext) : BaseRepository<Card>(dbContext), ICardRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Card>?> GetDeveloperCardsAsync(int devId)
    {
        return await _dbContext.Cards.Where(c => c.AssignedDeveloperId == devId).ToListAsync();
    }

    public async Task<List<Card>?> GetProjectCardsAsync(int projectId)
    {
        return await _dbContext.Cards.Where(c => c.ProjectId == projectId).ToListAsync();
    }
}