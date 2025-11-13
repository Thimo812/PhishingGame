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

    public DbSet<Email> Emails { get; set; }   // ✅ Explicit DbSet
    public DbSet<Training> Trainings { get; set; } // (optional, only if used later)


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Email>().ToTable("Emails");
        builder.Entity<Training>().ToTable("Trainings");
    }


}
