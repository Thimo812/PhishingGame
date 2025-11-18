using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhishingGame.Core.Models;

namespace PhishingGame.Data;

public class PhishingDbContext : IdentityDbContext
{
    public PhishingDbContext(DbContextOptions<PhishingDbContext> options)
        : base(options)
    {
    }

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
