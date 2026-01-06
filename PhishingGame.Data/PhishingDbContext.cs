using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhishingGame.Core.Models;

namespace PhishingGame.Data;

/// <summary>
/// Entity Framework database context for storing tables related to the phishing game and authentication thereof.
/// </summary>
/// <param name="options">
/// Configuration options for the database context.
/// </param>
public class PhishingDbContext(DbContextOptions<PhishingDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Email> Emails { get; set; }
    public DbSet<Training> Trainings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Training>()
            .HasMany(t => t.Emails)
            .WithMany(e => e.Trainings)
            .UsingEntity(j => j.ToTable("TrainingEmails"));
    }
}
