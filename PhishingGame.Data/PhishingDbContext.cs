using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhishinGame.Core.Models;
using System.Reflection;

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
}
