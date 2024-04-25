using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Opsphere.Data.Models;

namespace Opsphere.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Developer> Developers { get; set; }
    public DbSet<TeamLeader> TeamLeaders { get; set; }
}