using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repos;


public class CardRepository: ICardRepository
{
    private readonly ApplicationDbContext _context;

    public CardRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Card>> GetAllAsync()
    {
        return await _context.Cards.ToListAsync();
    }

    public async Task<Card> CreateAsync(Card cardModel)
    {
        await _context.Cards.AddAsync(cardModel);
        await _context.SaveChangesAsync();

        return cardModel;
    }
}