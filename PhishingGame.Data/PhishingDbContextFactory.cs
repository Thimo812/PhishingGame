using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PhishingGame.Data;

public class PhishingDbContextFactory : IDesignTimeDbContextFactory<PhishingDbContext>
{
    public PhishingDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<PhishingDbContext>()
            .UseSqlServer(string.Empty)
            .Options;

        return new PhishingDbContext(options);
    }
}
