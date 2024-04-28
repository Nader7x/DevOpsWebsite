using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Models;
using Opsphere.Interfaces;

namespace Opsphere.Data.Repositories;


public class CardRepository(ApplicationDbContext context) : BaseRepository<Card>(context), ICardRepository
{
    
}