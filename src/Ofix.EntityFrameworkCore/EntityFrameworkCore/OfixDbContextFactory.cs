using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ofix.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class OfixDbContextFactory : IDesignTimeDbContextFactory<OfixDbContext>
{
    public OfixDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        OfixEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<OfixDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new OfixDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Ofix.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
