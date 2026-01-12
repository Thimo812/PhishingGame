using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PhishingGame.Data;

public class PhishingDbContextFactory : IDesignTimeDbContextFactory<PhishingDbContext>
{
    public PhishingDbContext CreateDbContext(string[] args)
    {
        var config = GetConfig();
        var options = new DbContextOptionsBuilder<PhishingDbContext>();
        PhishingDbContext.ConfigureOptions(options, config);

        return new PhishingDbContext(options.Options);
    }

    private IConfiguration GetConfig()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
