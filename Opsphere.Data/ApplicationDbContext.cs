using System.Collections.Immutable;
using System.Runtime.CompilerServices;
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
        //composite key for the project developer table
        modelBuilder.Entity<ProjectDeveloper>()
            .HasKey(pd => new { pd.ProjectId, pd.UserId });
        modelBuilder.Entity<Card>()
            .HasOne(c => c.AssignedDeveloper)
            .WithMany(pd => pd.AssignedCards)
            .HasForeignKey(c => new { c.ProjectId, c.AssignedDeveloperId })
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
        modelBuilder.Entity<CardComment>()
            .HasOne(cm => cm.Card)
            .WithMany(c => c.CardComments)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade)
            .HasForeignKey(cm => cm.CardId);

        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Card)
            .WithOne(c => c.Attachment)
            .HasForeignKey<Attachment>(a => a.CardId);
        modelBuilder.Entity<CardComment>()
            .HasOne(cc => cc.User)
            .WithMany()
            .HasForeignKey(cc => cc.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
    }
    public DbSet<Project> Projects { get; init; }
    public DbSet<Card> Cards { get; init; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProjectDeveloper> ProjectDevelopers { get; set; }
    public DbSet<CardComment> CardComments { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

}