using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Interfaces;
using Opsphere.Data.Models;

namespace Opsphere.Data.Repositories;


public class CardRepository(ApplicationDbContext dbContext) : BaseRepository<Card>(dbContext), ICardRepository
{
    
}